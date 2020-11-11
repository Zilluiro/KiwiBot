using KiwiBot.Abstractions;
using Newtonsoft.Json;

namespace KiwiBot.DataModels
{
    class DanbooruPostModel: AbstractPostModel
    {
        [JsonProperty(PropertyName = "tag_string")]
        public override string Tags { get; set; }

        [JsonProperty(PropertyName = "file_url")]
        public override string FileUrl { get; set; }
    }
}
