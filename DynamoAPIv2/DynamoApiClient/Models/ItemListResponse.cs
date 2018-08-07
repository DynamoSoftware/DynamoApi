using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class ItemListResponse : ApiResponse
    {
        [JsonProperty("data")]
        public IEnumerable<IDictionary<string, object>> Data { get; set; }

        [JsonProperty("total")]
        public long? Total { get; set; }
    }
}