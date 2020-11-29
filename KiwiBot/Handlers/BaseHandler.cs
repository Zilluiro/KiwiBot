using KiwiBot.Helpers;

namespace KiwiBot.Handlers
{
    abstract class BaseHandler
    {
        public QueryContext Context { get; set; }
    }
}
