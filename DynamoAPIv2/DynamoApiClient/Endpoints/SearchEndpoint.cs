using System.Collections.Generic;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;
using DynamoApiClient.Models;

namespace DynamoApiClient.Endpoints
{
    public class SearchEndpoint
    {
        private readonly Client _client;
        public string[] PropertiesToRetrieve { get; set; }
        public object Advf { get; set; }

        public SearchEndpoint(Client client, object advf, params string[] propertiesToRetrieve)
        {
            Advf = advf;
            _client = client;
            PropertiesToRetrieve = propertiesToRetrieve;
        }

        public SearchEndpoint(Client client, IDictionary<string, object> advf, params string[] propertiesToRetrieve)
        {
            Advf = advf;
            _client = client;
            PropertiesToRetrieve = propertiesToRetrieve;
        }

        public SearchEndpoint ForProperties(params string[] properties)
        {
            return new SearchEndpoint(_client, Advf, properties);
        }

        public IEnumerable<DynamoItem> Items =>
            _client
                .MakeSearch(Advf, propertiesToRetrieve:PropertiesToRetrieve)
                .AsPage(_client)
                .ThrowIfErrorResponse()
                .GetAll();

    }
}
