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
        public async Task EchoCommandAsync()
        {
            TelegramBotClient client = Context.TelegramBotClient;

            await client.SendChatActionAsync(Context.Message.Chat.Id, ChatAction.Typing);
            await client.SendTextMessageAsync(Context.Message.Chat.Id, "pong");
        }

        [Registered]
        [Command("/last")]
        public async Task LastCommandAsync()
        {
            TelegramBotClient client = Context.TelegramBotClient;
            try
            {
                await client.SendChatActionAsync(Context.Chat.ChatId, ChatAction.UploadPhoto);

                BasePostModel post = await _messageService.GetLastPictureAsync(Context.Chat) ?? throw new Exception("no data available");
                await client.SendPhotoAsync(Context.Chat.ChatId, new InputOnlineFile(post.FileUrl), post.Tags);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("/random")]
        public async Task RandomCommandAsync()
        {
            TelegramBotClient client = Context.TelegramBotClient;
            try
            {
                await client.SendChatActionAsync(Context.Chat.ChatId, ChatAction.UploadPhoto);

                BasePostModel post = await _messageService.GetRandomPictureAsync(Context.Chat) ?? throw new Exception("no data available");
                await client.SendPhotoAsync(Context.Chat.ChatId, new InputOnlineFile(new Uri(post.FileUrl)), post.Tags);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("/start")]
        public async Task StartCommandAsync()
        {
            TelegramBotClient client = Context.TelegramBotClient;

            try
            {
                await _chatService.RegisterChatAsync(Context.Message.Chat.Id);
            }
            catch (Exception e)
            {
                await client.SendTextMessageAsync(Context.Message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("/settings")]
        public async Task SettingsCommandAsync()
        {
            TelegramBotClient client = Context.TelegramBotClient;

            try
            {
                await client.SendChatActionAsync(Context.Message.Chat.Id, ChatAction.Typing);
                Booru selectedBooru = await _chatService.GetSelectedBooruAsync(Context.Chat.ChatId);

                await client.SendTextMessageAsync(
                    chatId: Context.Chat.ChatId,
                    text: $"Current mode is {Context.Chat.ChatMode}\n" +
                          $"Current source is {selectedBooru.BooruName}",
                    replyMarkup: InlineKeyboards.settingsKeyboard,
                    replyToMessageId: Context.Message.MessageId
                );
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
