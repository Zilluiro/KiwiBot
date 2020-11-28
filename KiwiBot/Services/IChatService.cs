using KiwiBot.Data.Entities;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IChatService
    {
        Task<Chat> FindChatAsync(long chatId);
        Task<Booru> GetSelectedBooruAsync(long chatId);
        Task RegisterChatAsync(long chatId);
        Task UpdateChatModeAsync(Chat chat, string mode);
        Task UpdateChoosenBooruAsync(Chat chat, string booru);
    }
}
