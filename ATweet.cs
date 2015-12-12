using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lucas_Ebooks
{
    [JsonObject]
    class ATweet
    {
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "entities")]
        public ATweetEntities Entities { get; set; }
        [JsonProperty(PropertyName = "geo")]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreationTime { get; set; }
        [JsonProperty(PropertyName = "user")]
        public ATwitterUser Author { get; set; }

    }
}
