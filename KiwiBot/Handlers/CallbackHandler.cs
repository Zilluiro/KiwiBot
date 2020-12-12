using KiwiBot.Attributes;
using KiwiBot.Constants;
using KiwiBot.Data.Entities;
using KiwiBot.Helpers;
using KiwiBot.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace KiwiBot.Handlers
{
    class CallbackHandler: BaseHandler
    {
        private readonly IBooruService _booruService;
        private readonly IChatService _chatService;
        private readonly ILogger<CallbackHandler> _logger;
        private QueryCallbackContext callbackContext => Context as QueryCallbackContext;

        public CallbackHandler(IChatService chatService, IBooruService booruService,
            ILogger<CallbackHandler> logger)
        {
            _booruService = booruService;
            _chatService = chatService;
            _logger = logger;
        }

        private async Task DisplayCurrentSettings()
        {
            Booru selectedBooru = await _chatService.GetSelectedBooruAsync(Context.Chat.ChatId);

            (string message, InlineKeyboardMarkup markup) = BuildSettingsMessage(Context.Chat, selectedBooru);
            await client.EditMessageTextAsync(
                chatId: Context.Chat.ChatId,
                Context.Message.MessageId,
                text: message,
                replyMarkup: markup
            );
        }

        [Registered]
        [Command("Choose mode")]
        public async Task ChangeChatModeCallbackAsync()
        {
            try
            {
                await _chatService.UpdateChatModeAsync(callbackContext.Chat, callbackContext.Callback.Data);
                await client.AnswerCallbackQueryAsync(callbackContext.Callback.Id, $"{callbackContext.Callback.Data} mode enabled");
                await DisplayCurrentSettings();
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("Change mode")]
        public async Task DisplayChatModesAsync()
        {
            try
            {
                await client.EditMessageTextAsync(chatId: Context.Chat.ChatId, Context.Message.MessageId, "Choose mode");
                await client.EditMessageReplyMarkupAsync(chatId: Context.Chat.ChatId, Context.Message.MessageId, InlineKeyboards.modeKeyboard);
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("Choose source")]
        public async Task ChangeBooruCallbackAsync()
        {
            try
            {   
                await _chatService.UpdateChoosenBooruAsync(callbackContext.Chat, callbackContext.Callback.Data);
                await client.AnswerCallbackQueryAsync(callbackContext.Callback.Id, $"{callbackContext.Callback.Data} chosen");
                await DisplayCurrentSettings();
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Registered]
        [Command("Change source")]
        public async Task DisplayBoorusCallbackAsync()
        {
            try
            {   
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                List<Booru> boorus = await _booruService.GetBoorusAsync();
                boorus.ForEach(x => buttons.Add(InlineKeyboardButton.WithCallbackData(x.BooruName, $"/{x.BooruName}")));

                await client.EditMessageTextAsync(chatId: Context.Chat.ChatId, Context.Message.MessageId, "Choose source");
                await client.EditMessageReplyMarkupAsync(chatId: Context.Chat.ChatId, Context.Message.MessageId, new InlineKeyboardMarkup(new[] { buttons.ToArray()}));
            }
            catch(Exception e)
            {
                await client.SendTextMessageAsync(Context.Chat.ChatId, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
