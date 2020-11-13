using KiwiBot.BooruClients.Factories;
using KiwiBot.Data.Entities;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using KiwiBot.Helpers.Converters;
using KiwiBot.Services.Implementations;
using System.Net.Http;

namespace KiwiBot.BooruClients
{
    class CompatibleBooruFactory : IAbstractBooruFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public string FactoryName => "Compatible";

        public CompatibleBooruFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public AbstractBooruClient CreateBooruClient(Booru booru)
        {
            BooruClientConfiguration configuration = new BooruClientConfiguration
            {
                ApiEndpoint = booru.ApiEndpoint,
                FileUrlKey = booru.FileUrlKey,
                TagsKey = booru.TagsKey
            };

            var settings = JsonModelConverter.GenerateSerializerSettings<CompatiblePostModel>(configuration);
            return new CompatibleBooruClient(configuration, _httpClientFactory, settings);
        }
    }
}
