using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public abstract class TypedResponse<T> : ApiResponse
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}