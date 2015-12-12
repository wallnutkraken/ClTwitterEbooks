using Newtonsoft.Json;

namespace Lucas_Ebooks
{

    [JsonObject]
    class ATweetEntities
    {
        [JsonProperty(PropertyName = "user_mentions")]
        public string[] Mentions { get; set; }
        [JsonProperty(PropertyName = "media")]
        public string[] Media { get; set; }
        [JsonProperty(PropertyName = "hashtags")]
        public string[] HashTags { get; set; }
        [JsonProperty(PropertyName = "urls")]
        public string[] Urls { get; set; }
    }
}
