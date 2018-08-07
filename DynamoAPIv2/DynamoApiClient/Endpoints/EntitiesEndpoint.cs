using System.Collections.Generic;
using System.Linq;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;

namespace DynamoApiClient.Endpoints
{
    public class EntitiesEndpoint
    {
        private readonly Client _client;

        internal EntitiesEndpoint(Client client)
        {
            _client = client;
        }

        public EntityEndpoint this[string entityName] => new EntityEndpoint(_client, entityName);

        public IEnumerable<EntityEndpoint> All =>
            _client.GetEntities().ThrowIfErrorResponse().Data.Select(entityName => new EntityEndpoint(_client, entityName));
    }
}