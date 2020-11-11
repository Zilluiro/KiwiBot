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
    class DanbooruService: AbstractHttpClient, IBooruService
    {
        public string BooruName { get; } = "Danbooru";

        public DanbooruService(IHttpClientFactory httpClientFactory): base(httpClientFactory, new JsonConverter())
        {
        }

        public async Task<AbstractPostModel> GetLastPictureAsync()
        {
            Dictionary<string, string> query = new Dictionary<string, string>()
            {
                { "limit", "1" }
            };

            var result = await RetrieveDataAsync<List<DanbooruPostModel>>(DanbooruConstants.AllPostsUrl, query);
            return result.FirstOrDefault();
        }

        public async Task<AbstractPostModel> GetRandomPictureAsync()
        {
            string randomUrl = $"{DanbooruConstants.ConretePostUrl}/random.json";

            return await RetrieveDataAsync<DanbooruPostModel>(randomUrl, null);
        }
    }
}
