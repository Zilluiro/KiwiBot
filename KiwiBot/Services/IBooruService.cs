using KiwiBot.Abstractions;
using System.Threading.Tasks;

namespace KiwiBot.Services
{
    interface IBooruService
    {
        string BooruName  {get; }
        Task<AbstractPostModel> GetLastPictureAsync();
        Task<AbstractPostModel> GetRandomPictureAsync();
    }
}
