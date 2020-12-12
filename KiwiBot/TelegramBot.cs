using KiwiBot.Attributes;
using KiwiBot.Extensions;
using KiwiBot.Handlers;
using KiwiBot.Helpers;
using KiwiBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Chat = KiwiBot.Data.Entities.Chat;

namespace KiwiBot
{
    partial class TelegramBot: IHostedService
    {
        private readonly TelegramBotClient _telegramBot;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TelegramBot> _logger;
        private readonly Dictionary<Type, Dictionary<string, MethodInfo>> _registeredHandlers = new Dictionary<Type, Dictionary<string, MethodInfo>>();

        public TelegramBot(IServiceScopeFactory scopeFactory, IOptions<BotSettings> configuration, 
            ILogger<TelegramBot> logger)
        {
            _scopeFactory = scopeFactory;

            _telegramBot = new TelegramBotClient(configuration.Value.Token);
            _logger = logger;
        }

        public void Start()
        {
            Configure();

            _telegramBot.StartReceiving();
            Console.ReadLine();
            _telegramBot.StopReceiving();
        }

        private void Configure()
        {
            RegisterCommands();

            _telegramBot.OnMessage += (sender, e) => ProcessMessageAsync(sender, e, typeof(MessageHandler));
            _telegramBot.OnCallbackQuery += (sender, e) => ProcessCallbacksAsync(sender, e, typeof(CallbackHandler));
        }

        private void RegisterCommands()
        {
            Assembly.GetEntryAssembly().GetTypesAssignableFrom<BaseHandler>().ForEach((type) =>
            {
                Dictionary<string, MethodInfo> commands = new Dictionary<string, MethodInfo>();
                MethodInfo[] methods = type.GetMethods();

                foreach(MethodInfo method in methods)
                {
                    CommandAttribute attribute = method.GetCustomAttributes(true).FirstOrDefault(x => x is CommandAttribute == true) as CommandAttribute;

                    if (attribute is not object)
                        continue;

                    attribute.Commands.ForEach((command) => commands.Add(command, method));
                }

                _registeredHandlers.Add(type, commands);
            });
        }

        private async Task ExecuteMethodAsync(MethodInfo command, object instance, object[] arguments = null)
        {
            bool isAwaitable = command.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

            if (isAwaitable)
                await (Task)command.Invoke(instance, arguments);
            else
                command.Invoke(this, arguments);
        }

        private static async Task<(Chat chat, bool required)> CheckRegistration(MethodInfo foundCommand, IChatService chatService, long chatId)
        {
            RegisteredAttribute attribute = foundCommand.GetCustomAttributes(true).OfType<RegisteredAttribute>().FirstOrDefault();
            return (attribute is object) ? (await chatService.FindChatAsync(chatId), true) : default;
        }

        private async Task ProcessCommand(Type handler, QueryContext context)
        {
            Dictionary<string, MethodInfo> allCommands = _registeredHandlers[handler] ?? throw new Exception("handler not found");

            if (allCommands.ContainsKey(context.Command))
            {
                MethodInfo foundCommand = allCommands[context.Command];
                long chatId = context.Message.Chat.Id;

                using(var scope = _scopeFactory.CreateScope())
                {
                    (Chat chat, bool required) = await CheckRegistration(foundCommand, scope.ServiceProvider.GetService<IChatService>(), chatId);
                    if (chat == null && required) {
                        await _telegramBot.SendTextMessageAsync(chatId, "press /start to register chat");
                        return;
                    }
                    context.Chat = chat;
                    
                    object instance = scope.ServiceProvider.GetService(handler);
                    (instance as BaseHandler).Context = context;

                    await ExecuteMethodAsync(foundCommand, instance);
                }
            }
        }

        private async void ProcessMessageAsync(object sender, MessageEventArgs messageEventArgs, Type handler)
        {
            Message message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            string command = message.Text.Split(' ').First();
            _logger.LogInformation($"received command: {command}");

            QueryContext context = new QueryContext
            {
                Command = command,
                Message = message,
                TelegramBotClient = _telegramBot,
            };
            await ProcessCommand(handler, context);
        }

        private async void ProcessCallbacksAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs, Type handler)
        {
            CallbackQuery query = callbackQueryEventArgs.CallbackQuery;

            _logger.LogInformation($"received callback: {query.Id} {query.Data} {query.Message.Text}");

            string command = query.Data;
            if (command.StartsWith('/'))
            {
                command = query.Message.Text;
                query.Data = query.Data[1..];
            }
            
            QueryCallbackContext context = new QueryCallbackContext
            {
                Command = command,
                Message = query.Message,
                TelegramBotClient = _telegramBot,
                Callback = query,
            };
            await ProcessCommand(handler, context);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
