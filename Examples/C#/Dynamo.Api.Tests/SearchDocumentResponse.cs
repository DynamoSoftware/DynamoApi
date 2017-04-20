using Newtonsoft.Json;

namespace Dynamo.Api.Tests
{
    public class SearchDocumentResponse
    {
        [JsonProperty("next")]
        public string NextToken { get; set; }

        [JsonProperty("items")]
        public SearchDocumentResponseItem[] Items { get; set; }
    }
}