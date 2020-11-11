using KiwiBot.Abstractions;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IMessageService
    {
        Task<AbstractPostModel> GetLastPictureAsync();
        Task<AbstractPostModel> GetRandomPictureAsync();
    }
}
