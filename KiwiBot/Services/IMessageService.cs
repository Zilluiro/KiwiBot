using KiwiBot.Data.Entities;
using KiwiBot.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IMessageService
    {
        Task<BasePostModel> GetLastPictureAsync(Chat chat);
        Task<BasePostModel> GetRandomPictureAsync(Chat chat);
    }
}
