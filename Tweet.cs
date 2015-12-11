using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lucas_Ebooks
{
    class Tweet
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
