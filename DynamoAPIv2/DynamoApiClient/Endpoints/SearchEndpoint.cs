using System.Collections.Generic;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;

namespace DynamoApiClient.Endpoints
{
    public class SearchEndpoint
    {
        private readonly Client _client;
        public string[] PropertiesToRetrieve { get; set; }
        public IDictionary<string, object> Advf { get; set; }

        public SearchEndpoint(Client client, IDictionary<string, object> advf = null, string[] propertiesToRetrieve = null)
        {
            Advf = advf;
            _client = client;
            PropertiesToRetrieve = propertiesToRetrieve;
        }

        public IEnumerable<IDictionary<string, object>> Items =>
            _client
                .MakeSearch(Advf, PropertiesToRetrieve)
                .ThrowIfErrorResponse().Data;

    }
}
