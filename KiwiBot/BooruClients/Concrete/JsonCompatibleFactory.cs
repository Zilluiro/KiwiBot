using KiwiBot.BooruClients.Abstract;
using KiwiBot.Data.Entities;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using KiwiBot.Helpers.Converters;
using System.Net.Http;

namespace KiwiBot.BooruClients
{
    class JsonCompatibleFactory : IAbstractBooruFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public string Engine => "DanbooruMoebooru";

        public JsonCompatibleFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public AbstractBooruClient CreateBooruClient(Booru booru)
        {
            BooruClientConfiguration configuration = new BooruClientConfiguration
            {
                ApiEndpoint = booru.ApiEndpoint,
                FileUrlKey = booru.Engine.FileUrlKey,
                TagsKey = booru.Engine.TagsKey
            };

            var settings = JsonModelConverter.GenerateSerializerSettings<JsonCompatiblePostModel>(configuration);
            return new JsonCompatibleClient(configuration, _httpClientFactory, settings);
        }
    }
}
