using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WKApp.Models
{
    public class IdData
    {
        [JsonProperty("Character", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("MainReading", NullValueHandling = NullValueHandling.Ignore)]
        public string MainReading { get; set; }
        [JsonProperty("Level", NullValueHandling = NullValueHandling.Ignore)]
        public string Level { get; set; }
    }
}
