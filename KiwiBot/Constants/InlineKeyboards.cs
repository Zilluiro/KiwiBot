using Telegram.Bot.Types.ReplyMarkups;

namespace KiwiBot.Constants
{
    static class InlineKeyboards
    {
        public static readonly InlineKeyboardMarkup modeKeyboard = new InlineKeyboardMarkup(
            new []
            {
                InlineKeyboardButton.WithCallbackData("SFW",  "/SFW"),
                InlineKeyboardButton.WithCallbackData("NSFW", "/NSFW"),
            }
        );


        public static readonly InlineKeyboardMarkup settingsKeyboard = new InlineKeyboardMarkup(
            new []
            {
                InlineKeyboardButton.WithCallbackData("Change source"),
                InlineKeyboardButton.WithCallbackData("Change mode"),
            }
        );
    }
}
