using Newtonsoft.Json;

namespace Dynamo.Api.Tests
{
    internal class LoginRequest
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("tenant")]
        public string Tenant { get; set; }
    }
}