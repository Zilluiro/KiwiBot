using KiwiBot.Abstractions;
using KiwiBot.Attributes;
using KiwiBot.DataModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace KiwiBot
{
    partial class TelegramBot
    {
        [Command("/ping", "/test")]
        public async Task EchoCommand(object sender, Message message)
        {
            if (!(sender is TelegramBotClient))
                return;

            await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await _telegramBot.SendTextMessageAsync(message.Chat.Id, "pong");
        }

        [Command("/last")]
        public async Task LastCommand(object sender, Message message)
        {
            if (!(sender is TelegramBotClient))
                return;

            try
            {
                await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);
                AbstractPostModel post = await _messageService.GetLastPictureAsync() ?? throw new Exception("no data available");
                await _telegramBot.SendPhotoAsync(message.Chat.Id, new InputOnlineFile(post.FileUrl), post.Tags.ToString());
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("/random")]
        public async Task RandomCommand(object sender, Message message)
        {
            if (!(sender is TelegramBotClient))
                return;

            try
            {
                await _telegramBot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);
                AbstractPostModel post = await _messageService.GetRandomPictureAsync() ?? throw new Exception("no data available");
                await _telegramBot.SendPhotoAsync(message.Chat.Id, new InputOnlineFile(new Uri(post.FileUrl)), post.Tags.ToString());
            }
            catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
