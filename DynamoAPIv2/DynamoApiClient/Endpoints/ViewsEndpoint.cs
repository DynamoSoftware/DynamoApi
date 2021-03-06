﻿using System.Collections.Generic;
using System.Linq;
using DynamoApiClient.Clients;
using DynamoApiClient.Extensions;
using DynamoApiClient.Models;

namespace DynamoApiClient.Endpoints
{
    public class ViewsEndpoint
    {
        private readonly Client _client;

        internal ViewsEndpoint(Client client)
        {
            _client = client;
        }

        public IEnumerable<DynamoItem> this[string viewPath] =>
            _client.GetAllViewItems(viewPath).ThrowIfErrorResponse().Data;

        public IEnumerable<ViewEndpoint> All =>
            _client.GetViews().ThrowIfErrorResponse().Data.Select(viewItem => new ViewEndpoint(_client, viewItem));
    }
}