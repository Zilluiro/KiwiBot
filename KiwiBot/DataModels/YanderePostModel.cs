using KiwiBot.Abstractions;
using Newtonsoft.Json;

namespace KiwiBot.DataModels
{
    class YanderePostModel: AbstractPostModel
    {
        [JsonProperty(PropertyName = "tags")]
        public override string Tags { get; set; }

        [JsonProperty(PropertyName = "sample_url")]
        public override string FileUrl { get; set; }
    }
}
