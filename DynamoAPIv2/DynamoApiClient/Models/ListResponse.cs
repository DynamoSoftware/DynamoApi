using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class ListResponse : ApiResponse
    {
        [JsonProperty("data")]
        public IEnumerable<string> Data { get; set; }
    }
}