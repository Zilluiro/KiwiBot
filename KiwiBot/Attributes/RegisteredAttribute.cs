using System;

namespace KiwiBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class RegisteredAttribute: Attribute
    {
/*        private readonly IChatService _registerService;
        private bool _checked = false;
        private Chat _chat;*/

        public RegisteredAttribute()
        {
        }

/*        public async Task<Chat> GetChatAsync(long chatId)
        {
            if (_chat == default && !_checked)
            {
                _checked = true;
                _chat = await _registerService.FindChatAsync(chatId);
            }

            return _chat;
        }*/

/*        public async Task<bool> IsRegistered(long chatId)
        {
            Chat chat = await GetChatAsync(chatId);
            return chat is object;
        }*/
    }
}
