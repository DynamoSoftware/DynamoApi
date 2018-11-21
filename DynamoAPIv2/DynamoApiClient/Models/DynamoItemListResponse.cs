using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class DynamoItemListResponse : TypedResponse<DynamoItem[]>
    {
        [JsonProperty("total")]
        public long? Total { get; set; }
    }
}