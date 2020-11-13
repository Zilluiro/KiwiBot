﻿using KiwiBot.Data.Enumerations;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using KiwiBot.Helpers.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KiwiBot.BooruClients
{
    class CompatibleBooruClient : AbstractBooruClient
    {
        public CompatibleBooruClient(BooruClientConfiguration configuration, IHttpClientFactory httpClientFactory, JsonSerializerSettings settings)
            : base(httpClientFactory, configuration, new JsonModelConverter(settings))
        {
        }

        public async override Task<AbstractPostModel> GetLastPictureAsync(ChatModeEnum mode)
        {
            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "limit", "1" }
            };

            List<string> tags = new List<string>();
            if (mode == ChatModeEnum.SFW)
                tags.Add("rating:safe");

            query = AddTags(query, tags);
            var result = await RetrieveDataAsync<List<CompatiblePostModel>>(_configuration.ApiEndpoint, query);
            return result.FirstOrDefault();
        }

        public async override Task<AbstractPostModel> GetRandomPictureAsync(ChatModeEnum mode)
        {
            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "limit", "1" }
            };

            List<string> tags = new List<string>() { "order:random" };
            if (mode == ChatModeEnum.SFW)
                tags.Add("rating:safe");

            query = AddTags(query, tags);
            var result = await RetrieveDataAsync<List<CompatiblePostModel>>(_configuration.ApiEndpoint, query);

            return result.FirstOrDefault();
        }
    }
}
