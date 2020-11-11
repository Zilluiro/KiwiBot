using KiwiBot.Attributes;
using KiwiBot.Helpers;
using KiwiBot.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KiwiBot
{
    partial class TelegramBot
    {
        private readonly TelegramBotClient _telegramBot;
        private readonly IMessageService _messageService;
        private readonly Dictionary<string, MethodInfo> _registeredCommands;
        private readonly ILogger<TelegramBot> _logger;

        public TelegramBot(IOptions<BotSettings> configuration, IMessageService messageService, ILogger<TelegramBot> logger)
        {
            _telegramBot = new TelegramBotClient(configuration.Value.Token);
            _messageService = messageService;
            _registeredCommands = new Dictionary<string, MethodInfo>();
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

            _telegramBot.OnMessage += ProcessMessage;
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

        private async void ProcessMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Message message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            
            string command = message.Text.Split(' ').First();
            _logger.LogInformation($"received command: {command}");

            if (_registeredCommands.ContainsKey(command))
            {
                MethodInfo foundCommand = _registeredCommands[command];
                object[] arguments = new object[] {_telegramBot, message };
                bool isAwaitable = foundCommand.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

                if (isAwaitable)
                    await (Task)foundCommand.Invoke(this, arguments);
                else
                    foundCommand.Invoke(this, arguments);
            }
        }
    }
}
