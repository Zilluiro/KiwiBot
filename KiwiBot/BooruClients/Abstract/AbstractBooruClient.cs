using KiwiBot.Data.Enumerations;
using KiwiBot.DataModels;
using KiwiBot.Helpers;
using KiwiBot.Helpers.Converters;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KiwiBot.BooruClients.Abstract
{
    abstract class AbstractBooruClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDataConverter _dataConverter;
        protected readonly BooruClientConfiguration _configuration;

        public AbstractBooruClient(IHttpClientFactory httpClientFactory, BooruClientConfiguration configuration,
            IDataConverter dataConverter)
        {
            _httpClientFactory = httpClientFactory;
            _dataConverter = dataConverter;
            _configuration = configuration;
        }

        protected static Dictionary<string, string> AddTags(Dictionary<string, string> dict, List<string> tags)
        {
            if (tags.Count > 0)
                dict.Add("tags", tags.Aggregate((left, right) => $"{left} {right}"));
            return dict;
        }

        protected async Task<TModel> RetrieveDataAsync<TModel>(string url, Dictionary<string, string> query) where TModel : class
        {
            try
            {
                HttpResponseMessage reponse = await MakeRequestAsync(url, query);

                if (reponse.IsSuccessStatusCode)
                {
                    string content = await reponse.Content.ReadAsStringAsync();
                    TModel model = _dataConverter.From<TModel>(content);
                    return model;
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception("cannot retrieve data from api");
            }
        }

        protected async Task<string> RetrieveDataAsync(string url, Dictionary<string, string> query)
        {
            try
            {
                HttpResponseMessage reponse = await MakeRequestAsync(url, query);

                if (reponse.IsSuccessStatusCode)
                {
                    return await reponse.Content.ReadAsStringAsync();
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception("cannot retrieve data from api");
            }
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string url, Dictionary<string, string> query)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            string finalUrl = query is object ? QueryHelpers.AddQueryString(url, query) : url;
            HttpResponseMessage response = await client.GetAsync(finalUrl);

            return response;
        }

        public abstract Task<BasePostModel> GetLastPictureAsync(ChatModeEnum mode);
        public abstract Task<BasePostModel> GetRandomPictureAsync(ChatModeEnum mode);
    }
}
