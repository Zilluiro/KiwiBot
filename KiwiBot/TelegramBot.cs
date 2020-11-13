using KiwiBot.Attributes;
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

namespace KiwiBot
{
    partial class TelegramBot: IHostedService
    {
        private readonly TelegramBotClient _telegramBot;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TelegramBot> _logger;
        private readonly Dictionary<string, MethodInfo> _registeredCommands = new Dictionary<string, MethodInfo>();

        public TelegramBot(IServiceScopeFactory scopeFactory, IOptions<BotSettings> configuration, 
            ILogger<TelegramBot> logger)
        {
            _scopeFactory = scopeFactory;

            _telegramBot = new TelegramBotClient(configuration.Value.Token);
            _logger = logger;
        }

        public void Start()
        {
            ConfigureAsync();

            _telegramBot.StartReceiving();
            Console.ReadLine();
            _telegramBot.StopReceiving();
        }

        private void ConfigureAsync()
        {
            RegisterCommands();

            _telegramBot.OnMessage += ProcessMessageAsync;
            _telegramBot.OnCallbackQuery += ProcessCallbacksAsync;
        }

        private void RegisterCommands()
        {
            MethodInfo[] methods = typeof(TelegramBot).GetMethods();

            foreach(MethodInfo method in methods)
            {
                object[] attributes = method.GetCustomAttributes(true);
                foreach(Attribute attribute in attributes)
                {
                    if (attribute is CommandAttribute)
                    {
                        CommandAttribute commandAttribute = attribute as CommandAttribute;

                        foreach(string command in commandAttribute.Commands)
                            _registeredCommands.Add(command, method);
                    }
                }
            }
        }

        private bool ParametersContainsMessageService(ParameterInfo[] parameters)
        {
            return parameters.Any(x => x.ParameterType.FullName == typeof(IMessageService).FullName);
        }

        private async Task ExecuteMethodAsync(MethodInfo command, object[] arguments)
        {
            bool isAwaitable = command.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

            if (isAwaitable)
                await (Task)command.Invoke(this, arguments);
            else
                command.Invoke(this, arguments);
        }

        private async void ProcessMessageAsync(object sender, MessageEventArgs messageEventArgs)
        {
            Message message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            
            string command = message.Text.Split(' ').First();
            _logger.LogInformation($"received command: {command}");

            if (_registeredCommands.ContainsKey(command))
            {
                MethodInfo foundCommand = _registeredCommands[command];
                List<object> arguments = new List<object> { message };

                using(var scope = _scopeFactory.CreateScope())
                {
                    if (ParametersContainsMessageService(foundCommand.GetParameters()))
                    {
                        IMessageService messageService = scope.ServiceProvider.GetService<IMessageService>();
                        arguments.Add(messageService);
                    }
                    
                    await ExecuteMethodAsync(foundCommand, arguments.ToArray());
                }
            }
        }

        private async void ProcessCallbacksAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            CallbackQuery query = callbackQueryEventArgs.CallbackQuery;

            _logger.LogInformation($"received callback: {query.Id} {query.Data}");
            string command = query.Message.Text;

            if (_registeredCommands.ContainsKey(command))
            {
                MethodInfo foundCommand = _registeredCommands[command];
                List<object> arguments = new List<object>{ query };

                using(var scope = _scopeFactory.CreateScope())
                {
                    if (ParametersContainsMessageService(foundCommand.GetParameters()))
                    {
                        IMessageService messageService = scope.ServiceProvider.GetService<IMessageService>();
                        arguments.Add(messageService);
                    }
                    
                    await ExecuteMethodAsync(foundCommand, arguments.ToArray());
                }
            }
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
