using Telegram.Bot;
using Telegram.Bot.Types;
using Chat = KiwiBot.Data.Entities.Chat;

namespace KiwiBot.Helpers
{
    class QueryContext
    {
        public Chat Chat { get; set; }

        public Message Message { get; set; }

        public string Command { get; set;}

        public TelegramBotClient TelegramBotClient { get; set; }
    }

    class QueryCallbackContext: QueryContext
    {
        public CallbackQuery Callback {get; set; }
    }
}
