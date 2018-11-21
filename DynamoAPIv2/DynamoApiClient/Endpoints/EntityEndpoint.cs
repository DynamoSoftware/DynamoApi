using System;
using System.Collections.Generic;
using System.Linq;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;
using DynamoApiClient.Models;

namespace DynamoApiClient.Endpoints
{
    public class EntityEndpoint
    {
        public string Name { get; }
        private readonly Client _client;
        private readonly string[] _propertiesToRetrieve;

        internal EntityEndpoint(Client client, string entityName, string[] propertiesToRetrieve = null)
        {
            Name = entityName;
            _client = client;
            _propertiesToRetrieve = propertiesToRetrieve;
        }

        public EntityEndpoint ForProperties(params string[] properties)
        {
            return new EntityEndpoint(_client, Name, properties);
        }

        public IDictionary<string, Type> Properties => _client
            .GetEntityProperties(Name)
            .ThrowIfErrorResponse()
            .Data
            .ToDictionary(k => k.Key, v => Type.GetType("System." + (string)v.Value));

        public IReadOnlyCollection<DynamoItem> Items =>
            (IReadOnlyCollection<DynamoItem>)_client.GetEntityItems(Name, _propertiesToRetrieve)
                .AsPage(_client)
                .ThrowIfErrorResponse()
                .GetAll();

        public long Total => _client.GetAllEntityItemsCount(Name).ThrowIfErrorResponse().Data.Total;

        public DynamoItem this[string id] => GetById(id);

        public DynamoItem GetById(string id) =>
            _client.GetEntityItemById(Name, id, _propertiesToRetrieve).ThrowIfErrorResponse().Data;

        public void Delete(string id)
        {
            _client.DeleteEntityItem(Name, id).ThrowIfErrorResponse();
        }

        public DynamoItem Insert(IDictionary<string, object> item)
        {
            return _client.InsertEntityItem(Name, item).ThrowIfErrorResponse().Data;
        }

        public DynamoItem Update(string id, IDictionary<string, object> item)
        {
            return _client.UpdateEntityItem(Name, id, item).ThrowIfErrorResponse().Data;
        }

        public SchemaResponse.EntitySchema Schema => _client
            .GetEntitySchema(Name)
            .ThrowIfErrorResponse()
            .Data;
    }
}