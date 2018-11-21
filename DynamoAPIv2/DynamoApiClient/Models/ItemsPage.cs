using System;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;

namespace DynamoApiClient.Models
{
    public class ItemsPage<T> where T: ApiResponse, new()
    {
        public ApiCallResponse<T> Response { get; }
        private readonly Client _client;

        internal ItemsPage(Client client, ApiCallResponse<T> listResponse)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            Response = listResponse;
            this.ThrowIfErrorResponse();
        }

        public bool HasNext => Response?.Data?.Links?.Next != null;

        public ItemsPage<T> GetNext()
        {
            if (Response == null)
                return null;

            return HasNext
                ? new ItemsPage<T>(_client, _client.PlaceRequest<T>(Response.Data.Links.Next.AbsoluteUri))
                : new ItemsPage<T>(_client, null);
        }
    }
}