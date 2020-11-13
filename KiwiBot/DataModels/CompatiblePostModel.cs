using Newtonsoft.Json;

namespace KiwiBot.DataModels
{
    class CompatiblePostModel: AbstractPostModel
    {
        [JsonProperty(PropertyName = "tag")]
        public override string Tags { get; set; }

        [JsonProperty(PropertyName = "file")]
        public override string FileUrl { get; set; }
    }
}
