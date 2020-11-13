using Newtonsoft.Json;
using System;

namespace KiwiBot.Helpers.Converters
{
    class JsonModelConverter : IDataConverter
    {
        private readonly JsonSerializerSettings _settings;
        public JsonModelConverter(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string To<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T From<T>(string str) where T : class
        {
            return JsonConvert.DeserializeObject<T>(str, _settings);
        }

        public static JsonSerializerSettings GenerateSerializerSettings<T>(BooruClientConfiguration configuration)
        {
            var jsonResolver = new PropertyRenameSerializerContractResolver();

            if (configuration.FileUrlKey == null || configuration.TagsKey == null)
                throw new Exception("booru's keys are not specified");

            jsonResolver.RenameProperty(typeof(T), "file", configuration.FileUrlKey);
            jsonResolver.RenameProperty(typeof(T), "tag", configuration.TagsKey);

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = jsonResolver
            };

            return serializerSettings;
        }
    }
}
