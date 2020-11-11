using KiwiBot.Abstractions;
using KiwiBot.Constants;
using KiwiBot.DataModels;
using KiwiBot.Extensions.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KiwiBot.Services.Implementations
{
    class YandereBooruService: AbstractHttpClient, IBooruService
    {
        public string BooruName { get; } = "Yandere";

        private readonly string _url = YandereConstants.PostsUrl;

        public YandereBooruService(IHttpClientFactory httpClientFactory): base(httpClientFactory, new JsonConverter())
        {
        }

        public async Task<AbstractPostModel> GetLastPictureAsync()
        {
            Dictionary<string, string> query = new Dictionary<string, string>()
            {
                { "limit", "1" }
            };

            var response = await RetrieveDataAsync<List<YanderePostModel>>(_url, query);
            return response.FirstOrDefault();
        }

        public async Task<AbstractPostModel> GetRandomPictureAsync()
        {
            Dictionary<string, string> query = new Dictionary<string, string>()
            {
                { "tags", "order:random" }
            };

            var response = await RetrieveDataAsync<List<YanderePostModel>>(_url, query);
            return response.FirstOrDefault();
        }
    }
}
