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
            AbstractPostModel post = null;
            try
            {
                await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                post = await messageService.GetRandomPictureAsync(message.Chat.Id) ?? throw new Exception("no data available");
                await _telegramBot.SendPhotoAsync(message.Chat.Id, new InputOnlineFile(new Uri(post.FileUrl)), post.Tags);
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
                _logger.LogError(post.FileUrl);
            }
        }

        [Command("/start")]
        public async Task StartCommandAsync(Message message, IMessageService messageService)
        {
            try
            {
                Console.WriteLine(message.Chat.FirstName);
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
                    text: "Choose mode",
                    replyMarkup: chooseModeKeyboard
                );

                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                List<Booru> boorus = await messageService.GetBoorusAsync();
                boorus.ForEach(x => buttons.Add(InlineKeyboardButton.WithCallbackData(x.BooruName)));

                var chooseBooruKeyboard = new InlineKeyboardMarkup(new[] { buttons.ToArray() });

                await _telegramBot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose booru",
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
