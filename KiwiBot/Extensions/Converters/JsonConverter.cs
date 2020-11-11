using Newtonsoft.Json;

namespace KiwiBot.Extensions.Converters
{
    class JsonConverter : IDataConverter
    {
        public string To<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T From<T>(string str) where T : class
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
