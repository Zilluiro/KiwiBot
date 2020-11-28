using KiwiBot.Attributes;
using KiwiBot.Constants;
using KiwiBot.Data.Entities;
using KiwiBot.Helpers;
using KiwiBot.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KiwiBot.Handlers
{
    class CallbackHandler: BaseHandler
    {
        private readonly IBooruService _booruService;
        private readonly IChatService _chatService;
        private readonly ILogger<CallbackHandler> _logger;

        public CallbackHandler(IChatService chatService, IBooruService booruService,
            ILogger<CallbackHandler> logger)
        {
            _booruService = booruService;
            _chatService = chatService;
            _logger = logger;
        }

        private async Task DisplayCurrentSettings(QueryContext context)
        {
            Booru selectedBooru = await _chatService.GetSelectedBooruAsync(context.Chat.ChatId);

            await context.TelegramBotClient.EditMessageTextAsync(
                chatId: context.Chat.ChatId,
                context.Message.MessageId,
                text: $"Current mode is {context.Chat.ChatMode}\n" +
                        $"Current source is {selectedBooru.BooruName}",
                replyMarkup: InlineKeyboards.settingsKeyboard
            );
        }

        [Registered]
        [Command("Choose mode")]
        public async Task ChangeChatModeCallbackAsync(QueryCallbackContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            try
            {
                await _chatService.UpdateChatModeAsync(context.Chat, context.Callback.Data);
                await client.AnswerCallbackQueryAsync(context.Callback.Id, $"{context.Callback.Data} mode enabled");
                await DisplayCurrentSettings(context);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("Change mode")]
        public async Task DisplayChatModesAsync(QueryCallbackContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            try
            {
                await client.EditMessageTextAsync(chatId: context.Chat.ChatId, context.Message.MessageId, "Choose mode");
                await client.EditMessageReplyMarkupAsync(chatId: context.Chat.ChatId, context.Message.MessageId, InlineKeyboards.modeKeyboard);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("Choose source")]
        public async Task ChangeBooruCallbackAsync(QueryCallbackContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            try
            {   
                await _chatService.UpdateChoosenBooruAsync(context.Chat, context.Callback.Data);
                await client.AnswerCallbackQueryAsync(context.Callback.Id, $"{context.Callback.Data} chosen");
                await DisplayCurrentSettings(context);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("Change source")]
        public async Task DisplayBoorusCallbackAsync(QueryCallbackContext context)
        {
            TelegramBotClient client = context.TelegramBotClient;

            try
            {   
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                List<Booru> boorus = await _booruService.GetBoorusAsync();
                boorus.ForEach(x => buttons.Add(InlineKeyboardButton.WithCallbackData(x.BooruName, $"/{x.BooruName}")));

                await client.EditMessageTextAsync(chatId: context.Chat.ChatId, context.Message.MessageId, "Choose source");
                await client.EditMessageReplyMarkupAsync(chatId: context.Chat.ChatId, context.Message.MessageId, new InlineKeyboardMarkup(new[] { buttons.ToArray()}));
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
