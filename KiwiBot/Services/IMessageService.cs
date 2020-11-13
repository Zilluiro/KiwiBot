using KiwiBot.Data.Entities;
using KiwiBot.Data.Enumerations;
using KiwiBot.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IMessageService
    {
        Task<AbstractPostModel> GetLastPictureAsync(long chatId);
        Task<AbstractPostModel> GetRandomPictureAsync(long chatId);
        Task RegisterChatAsync(long chatId);
        Task UpdateChatModeAsync(long chatId, string mode);
        Task UpdateChoosenBooruAsync(long chatId, string booru);
        Task<List<Booru>> GetBoorusAsync();
        Task<Chat> FindChatAsync(long chatId);
        Task<Chat> FindChatWithIncludesAsync(long chatId);
    }
}
