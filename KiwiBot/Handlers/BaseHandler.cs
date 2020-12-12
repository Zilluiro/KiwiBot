using KiwiBot.Data.Entities;
using KiwiBot.Helpers;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KiwiBot.Handlers
{
    abstract class BaseHandler
    {
        public QueryContext Context { get; set; }

        protected TelegramBotClient client => Context.TelegramBotClient;

        protected (string message, InlineKeyboardMarkup markup) BuildSettingsMessage(Chat chat, Booru booru)
        {
            string message = $"Current source is {booru.BooruName}\n";
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("Change source") };

            string chatMode;
            if (booru.LockedMode)
            {
                chatMode = "Locked";
            }
            else
            {
                chatMode = chat.ChatMode.ToString();
                buttons.Add(InlineKeyboardButton.WithCallbackData("Change mode"));
            }

            message += $"Current mode is {chatMode}\n";
            return (message, new InlineKeyboardMarkup(buttons));
        }
    }
}
