using Newtonsoft.Json;

namespace KiwiBot.DataModels
{
    class JsonCompatiblePostModel: BasePostModel
    {
        [JsonProperty(PropertyName = "tag")]
        public override string Tags { get; set; }

        [JsonProperty(PropertyName = "file")]
        public override string FileUrl { get; set; }
    }
}
