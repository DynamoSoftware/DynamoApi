using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class ItemResponse : ApiResponse
    {
        [JsonProperty("data")]
        public IDictionary<string, object> Data { get; set; }
    }
}