using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class TotalResponse : TypedResponse<TotalResponse.TotalObject>
    {
        public class TotalObject
        {
            [JsonProperty("total")]
            public long Total { get; set; }
        }
    }
}