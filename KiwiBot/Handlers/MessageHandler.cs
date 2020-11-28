using KiwiBot.Attributes;
using KiwiBot.Constants;
using KiwiBot.Data.Entities;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using KiwiBot.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace KiwiBot.Handlers
{
    class MessageHandler: BaseHandler
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(IChatService chatService, IMessageService messageService, ILogger<MessageHandler> logger)
        {
            _chatService = chatService;
            _messageService = messageService;   
            _logger = logger;
        }

        [Registered]
        [Command("/ping", "/test")]
        public async Task EchoCommandAsync(QueryContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            await client.SendChatActionAsync(context.Message.Chat.Id, ChatAction.Typing);
            await client.SendTextMessageAsync(context.Message.Chat.Id, "pong");
        }

        [Registered]
        [Command("/last")]
        public async Task LastCommandAsync(QueryContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;
            try
            {
                await client.SendChatActionAsync(context.Chat.ChatId, ChatAction.UploadPhoto);

                AbstractPostModel post = await _messageService.GetLastPictureAsync(context.Chat) ?? throw new Exception("no data available");
                await client.SendPhotoAsync(context.Chat.ChatId, new InputOnlineFile(post.FileUrl), post.Tags);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("/random")]
        public async Task RandomCommandAsync(QueryContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;
            try
            {
                await client.SendChatActionAsync(context.Chat.ChatId, ChatAction.UploadPhoto);

                AbstractPostModel post = await _messageService.GetRandomPictureAsync(context.Chat) ?? throw new Exception("no data available");
                await client.SendPhotoAsync(context.Chat.ChatId, new InputOnlineFile(new Uri(post.FileUrl)), post.Tags);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("/start")]
        public async Task StartCommandAsync(QueryContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            try
            {
                await _chatService.RegisterChatAsync(context.Message.Chat.Id);
            }
            catch (Exception e)
            {
                await client.SendTextMessageAsync(context.Message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("/settings")]
        public async Task SettingsCommandAsync(QueryContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            try
            {
                await client.SendChatActionAsync(context.Message.Chat.Id, ChatAction.Typing);
                Booru selectedBooru = await _chatService.GetSelectedBooruAsync(context.Chat.ChatId);

                await client.SendTextMessageAsync(
                    chatId: context.Chat.ChatId,
                    text: $"Current mode is {context.Chat.ChatMode}\n" +
                          $"Current source is {selectedBooru.BooruName}",
                    replyMarkup: InlineKeyboards.settingsKeyboard,
                    replyToMessageId: context.Message.MessageId
                );
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
