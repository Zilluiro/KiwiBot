using KiwiBot.Attributes;
using KiwiBot.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace KiwiBot
{
    partial class TelegramBot
    {
        [Command("Choose mode")]
        public async Task ChangeChatModeCallbackAsync(CallbackQuery callback, IMessageService messageService)
        {
            try
            {
                await messageService.UpdateChatModeAsync(callback.Message.Chat.Id, callback.Data);
                
                await _telegramBot.AnswerCallbackQueryAsync(callback.Id, $"{callback.Data} mode enabled");
            }catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(callback.Message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }

        [Command("Choose booru")]
        public async Task ChangeBooruCallbackAsync(CallbackQuery callback, IMessageService messageService)
        {
            try
            {   
                await messageService.UpdateChoosenBooruAsync(callback.Message.Chat.Id, callback.Data);

                await _telegramBot.AnswerCallbackQueryAsync(callback.Id, $"{callback.Data} chosen");
            }catch(Exception e)
            {
                await _telegramBot.SendTextMessageAsync(callback.Message.Chat.Id, e.Message);
                _logger.LogError(e.Message);
            }
        }
    }
}
