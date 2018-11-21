using System.Collections.Generic;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;
using DynamoApiClient.Models;

namespace DynamoApiClient.Endpoints
{
    public class ViewEndpoint
    {
        private readonly Client _client;
        public string Name { get; }
        public string Path { get; set; }

        public ViewEndpoint(Client client, IDictionary<string, object> viewItem)
        {
            _client = client;
            Name = viewItem["name"] as string;
            Path = viewItem["path"] as string;
        }

        public IEnumerable<DynamoItem> Items =>
            _client.GetAllViewItems(Path).ThrowIfErrorResponse().Data;
    }
}