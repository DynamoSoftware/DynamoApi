using DynamoApiClient.Endpoints;
using DynamoApiClient.Extensions;
using System.Collections.Generic;

namespace DynamoApiClient.Clients
{
    public class FluentClient
    {
        public Client LowLevelClient { get; }

        public FluentClient(Client lowLevelClient)
        {
            LowLevelClient = lowLevelClient;
        }

        public FluentClient(string apiKey, string apiUrl = "http://localhost:51516/api/")
        {
            LowLevelClient = new Client(apiKey, apiUrl);
        }

        public EntitiesEndpoint Entities => new EntitiesEndpoint(LowLevelClient);

        public ViewsEndpoint Views => new ViewsEndpoint(LowLevelClient);

        public IDictionary<string, object> Version => LowLevelClient
            .GetVersion()
            .ThrowIfErrorResponse()
            .Data;

        public bool Reset()
        {
            return LowLevelClient.Reset().Success;
        }
    }
}