using KiwiBot.Attributes;
using KiwiBot.Data.Entities;
using KiwiBot.DataModels;
using KiwiBot.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace KiwiBot
{
    partial class TelegramBot
    {
        [Command("/ping", "/test")]
        public async Task EchoCommandAsync(Message message)
        {
            await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await _telegramBot.SendTextMessageAsync(message.Chat.Id, "pong");
        }

        [Command("/last")]
        public async Task LastCommandAsync(Message message, IMessageService messageService)
        {
            try
            {
                await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                AbstractPostModel post = await messageService.GetLastPictureAsync(message.Chat.Id) ?? throw new Exception("no data available");
                await _telegramBot.SendPhotoAsync(message.Chat.Id, new InputOnlineFile(post.FileUrl), post.Tags);
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("/random")]
        public async Task RandomCommandAsync(Message message, IMessageService messageService)
        {
            try
            {
                await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                AbstractPostModel post = await messageService.GetRandomPictureAsync(message.Chat.Id) ?? throw new Exception("no data available");
                await _telegramBot.SendPhotoAsync(message.Chat.Id, new InputOnlineFile(new Uri(post.FileUrl)), post.Tags);
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("/start")]
        public async Task StartCommandAsync(Message message, IMessageService messageService)
        {
            try
            {
                await messageService.RegisterChatAsync(message.Chat.Id);
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("/settings")]
        public async Task SettingsCommandAsync(Message message, IMessageService messageService)
        {
            try
            {
                await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                Data.Entities.Chat currentChat = await messageService.FindChatWithIncludesAsync(message.Chat.Id);

                var chooseModeKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("SFW"),
                        InlineKeyboardButton.WithCallbackData("NSFW"),
                    }
                });

                await _telegramBot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Choose mode (Current mode is {currentChat?.ChatMode})",
                    replyMarkup: chooseModeKeyboard
                );

                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                List<Booru> boorus = await messageService.GetBoorusAsync();
                boorus.ForEach(x => buttons.Add(InlineKeyboardButton.WithCallbackData(x.BooruName)));

                var chooseBooruKeyboard = new InlineKeyboardMarkup(new[] { buttons.ToArray() });

                await _telegramBot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Choose booru (Current booru is {currentChat?.Booru.BooruName})",
                    replyMarkup: chooseBooruKeyboard
                );
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
