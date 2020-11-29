using KiwiBot.BooruClients.Abstract;
using KiwiBot.Data.Entities;
using KiwiBot.Helpers;
using KiwiBot.Helpers.Converters;
using System.Net.Http;

namespace KiwiBot.BooruClients
{
    class GelbooruCompatibleFactory : IAbstractBooruFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public string Engine => "Gelbooru";

        public GelbooruCompatibleFactory(IHttpClientFactory httpClientFactory)
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

            return new GelbooruCompatibleClient(configuration, _httpClientFactory, new XmlModelConverter());
        }
    }
}
