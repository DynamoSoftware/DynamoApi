using System;
using DynamoApiClient.Clients;
using DynamoApiClient.Models;

namespace DynamoApiClient
{
    public class ItemsPage
    {
        public ItemListResponse Response { get; }
        private readonly Client _client;

        internal ItemsPage(Client client, ItemListResponse listResponse)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            Response = listResponse;
        }

        public bool HasNext => Response?.Links?.Next != null;

        public ItemsPage GetNext()
        {
            if (Response == null)
                return null;

            return HasNext
                ? new ItemsPage(_client, _client.PlaceGetRequest<ItemListResponse>(Response.Links.Next.AbsoluteUri))
                : new ItemsPage(_client, null);
        }
    }
}