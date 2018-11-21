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

        public FluentClient(string apiKey, string apiUrl = "https://api.dynamosoftware.com/api/v2.0/")
        {
            LowLevelClient = new Client(apiKey, apiUrl);
        }

        public EntitiesEndpoint Entities => new EntitiesEndpoint(LowLevelClient);

        public ViewsEndpoint Views => new ViewsEndpoint(LowLevelClient);

        public IDictionary<string, object> Version => LowLevelClient
            .GetVersion()
            .ThrowIfErrorResponse()
            .Data;

        public SearchEndpoint Search(object advf, params string[] propertyNames)
        {
            return new SearchEndpoint(LowLevelClient, advf, propertyNames);
        }

        public bool Reset()
        {
            return LowLevelClient.Reset().Data.Success;
        }
    }
}