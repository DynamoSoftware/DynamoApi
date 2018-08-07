using System;
using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class Links
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }
    }
}