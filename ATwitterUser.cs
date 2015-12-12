using Newtonsoft.Json;

namespace Lucas_Ebooks
{
    [JsonObject]
    class ATwitterUser
    {
        [JsonProperty(PropertyName = "name")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
        [JsonProperty(PropertyName = "protected")]
        public bool IsProtected { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "profile_image_url_https")]
        public string ProfileImageUrl { get; set; }
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "verified")]
        public bool IsVerified { get; set; }
    }
}
