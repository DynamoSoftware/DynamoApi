using System.Collections.Generic;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;

namespace DynamoApiClient.Endpoints
{
    public class ResetEndpoint
    {
        private readonly Client _client;

        public ResetEndpoint(Client client)
        {
            _client = client;
        }

        public IDictionary<string, object> Reset =>
            _client
                .Reset()
                .ThrowIfErrorResponse().Data;
    }
}
