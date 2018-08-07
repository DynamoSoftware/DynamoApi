using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public abstract class ApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}