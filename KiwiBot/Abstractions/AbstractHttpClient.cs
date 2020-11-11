using KiwiBot.Extensions.Converters;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KiwiBot.Abstractions
{
    abstract class AbstractHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDataConverter _dataConverter;


        public AbstractHttpClient(IHttpClientFactory httpClientFactory, IDataConverter dataConverter)
        {
            _httpClientFactory = httpClientFactory;
            _dataConverter = dataConverter;
        }

        protected async Task<TModel> RetrieveDataAsync<TModel>(string url, Dictionary<string, string> query) where TModel: class
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
            catch(Exception e)
            {
                throw new Exception("cannot retrieve data from api");
            }
        }

        protected async Task<HttpResponseMessage> MakeRequestAsync(string url, Dictionary<string, string> query)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            string finalUrl = query is object ? QueryHelpers.AddQueryString(url, query): url;
            HttpResponseMessage response = await client.GetAsync(finalUrl);

            return response;
        }
    }
}
