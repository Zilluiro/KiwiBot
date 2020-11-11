using KiwiBot.Abstractions;
using KiwiBot.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiwiBot.Services.Implementations
{
    class MessageService: IMessageService
    {
        private readonly IBooruService _booruService;

        public MessageService(IEnumerable<IBooruService> services)
        {
            // select a datasource

            // _booruService = services.Where(service => service.BooruName == YandereConstants.Name).Single();
            _booruService = services.Where(service => service.BooruName == DanbooruConstants.Name).Single();
        }

        public async Task<AbstractPostModel> GetLastPictureAsync()
        {
            return await _booruService.GetLastPictureAsync();
        }

        public async Task<AbstractPostModel> GetRandomPictureAsync()
        {
            return await _booruService.GetRandomPictureAsync();
        }
    }
}
