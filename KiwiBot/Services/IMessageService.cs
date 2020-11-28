using KiwiBot.Data.Entities;
using KiwiBot.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IMessageService
    {
        Task<AbstractPostModel> GetLastPictureAsync(Chat chat);
        Task<AbstractPostModel> GetRandomPictureAsync(Chat chat);
    }
}
