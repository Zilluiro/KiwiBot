using AngleSharp;
using AngleSharp.Dom;
using KiwiBot.BooruClients.Abstract;
using KiwiBot.Data.Enumerations;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using KiwiBot.Helpers.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KiwiBot.BooruClients
{
    class GelbooruCompatibleClient: AbstractBooruClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDataConverter _dataConverter;

        public GelbooruCompatibleClient(BooruClientConfiguration configuration, IHttpClientFactory httpClientFactory, IDataConverter dataConverter)
            : base(httpClientFactory, configuration, dataConverter)
        {
            _httpClientFactory = httpClientFactory;
            _dataConverter = dataConverter;
        }

        public async override Task<BasePostModel> GetLastPictureAsync(ChatModeEnum mode)
        {
            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "limit", "1" },
                { "page", "dapi" },
                { "s", "post" },
                { "q", "index" },
            };

            List<string> tags = new List<string>();
            if (mode == ChatModeEnum.SFW)
                tags.Add("safe");

            query = AddTags(query, tags);
            var result = await RetrieveDataAsync<SafeBooruPostsModel>(_configuration.ApiEndpoint, query);
            GelbooruCompatiblePostModel lastPost = result.Posts.FirstOrDefault();

            return lastPost;
        }

        public override async Task<BasePostModel> GetRandomPictureAsync(ChatModeEnum mode)
        {
            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "page", "post" },
                { "s", "random" },
                { "limit", "1" },
            };

            List<string> tags = new List<string>();
            if (mode == ChatModeEnum.SFW)
                tags.Add("safe");

            query = AddTags(query, tags);
            string content = await RetrieveDataAsync(_configuration.ApiEndpoint, query);

            IBrowsingContext context = BrowsingContext.New(Configuration.Default);
            IDocument document = await context.OpenAsync(req => req.Content(content));
            IElement selector = document.QuerySelector("#image");

            string image = selector.GetAttribute("src");
            string imageTags = selector.GetAttribute("alt");

            return new BasePostModel
            {
                FileUrl = image,
                Tags = imageTags,
            };
        }
    }
}
