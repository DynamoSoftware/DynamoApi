using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DynamoApiClient.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace DynamoApiClient.Clients
{
    public class Client
    {
        private class JsonDeserializer : IDeserializer
        {
            public JsonDeserializer()
            {
                Culture = CultureInfo.InvariantCulture;
            }

            public T Deserialize<T>(IRestResponse response)
            {
                return JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings
                    {
                        Culture = Culture,
                        DateFormatString = DateFormat
                    });
            }

            public CultureInfo Culture { get; set; }

            public string RootElement { get; set; }
            public string Namespace { get; set; }
            public string DateFormat { get; set; }
        }

        private readonly string _apiKey;
        private readonly RestClient _client;

        public static TApiResponse PlaceGetRequest<TApiResponse>(string apiUrl, string apiKey) where TApiResponse : ApiResponse, new()
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("", Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            IRestResponse<TApiResponse> response = client.Execute<TApiResponse>(request);
            return response.Data;
        }

        public Client(string apiKey, string apiUrl = "http://localhost:51516/api/v2.0/")
        {
            _apiKey = apiKey;
            _client = new RestClient(apiUrl);
            _client.AddHandler("application/json", new JsonDeserializer());
        }

        internal TApiResponse PlaceGetRequest<TApiResponse>(string url) where TApiResponse : ApiResponse, new()
        {
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<TApiResponse> response = _client.Execute<TApiResponse>(request);
            return response.Data;
        }

        public ListResponse GetEntities()
        {
            var request = new RestRequest("Entity", Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ListResponse> response = _client.Execute<ListResponse>(request);
            return response.Data;
        }

        public long GetAllEntityItemsCount(string entityName)
        {
            var request = new RestRequest("Entity/{entityName}/total", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return (long)(response.Data.Data["total"]);
        }

        public ItemsPage GetEntityItems(string entityName, params string[] propertiesToRetrieve)
        {
            var request = new RestRequest("Entity/{entityName}", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            IRestResponse<ItemListResponse> response = _client.Execute<ItemListResponse>(request);
            return new ItemsPage(this, response.Data);
        }

        public ItemResponse GetEntityItemById(string entityName, Guid id, params string[] propertiesToRetrieve)
        {
            var request = new RestRequest("Entity/{entityName}/{id}", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddParameter("id", id.ToString("N"), ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse GetEntityItemById(string entityName, string id, params string[] propertiesToRetrieve)
        {
            var request = new RestRequest("Entity/{entityName}/{id}", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemListResponse GetAllEntityItems(string entityName, params string[] propertiesToRetrieve)
        {
            var request = new RestRequest("Entity/{entityName}", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddQueryParameter("all", "true");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            IRestResponse<ItemListResponse> response = _client.Execute<ItemListResponse>(request);
            return response.Data;
        }

        public ItemResponse GetEntityProperties(string entityName)
        {
            var request = new RestRequest("Entity/{entityName}/properties", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse GetEntitySchema(string entityName)
        {
            var request = new RestRequest("Entity/{entityName}/schema", Method.GET);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse DeleteEntityItem(string entityName, Guid id)
        {
            var request = new RestRequest("Entity/{entityName}/{id}", Method.DELETE);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddParameter("id", id.ToString("N"), ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse DeleteEntityItem(string entityName, string id)
        {
            var request = new RestRequest("Entity/{entityName}/{id}", Method.DELETE);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse InsertEntityItem(string entityName, IDictionary<string, object> item)
        {
            var request = new RestRequest("Entity/{entityName}", Method.POST);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddJsonBody(item);
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse UpdateEntityItem(string entityName, Guid id, IDictionary<string, object> item)
        {
            var request = new RestRequest("Entity/{entityName}/{id}", Method.PUT);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddParameter("id", id.ToString("N"), ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddJsonBody(item);
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse UpdateEntityItem(string entityName, string id, IDictionary<string, object> item)
        {
            var request = new RestRequest("Entity/{entityName}/{id}", Method.PUT);
            request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddJsonBody(item);
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemListResponse GetViews()
        {
            var request = new RestRequest("View", Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemListResponse> response = _client.Execute<ItemListResponse>(request);
            return response.Data;
        }

        public ItemListResponse GetAllViewItems(string path)
        {
            var request = new RestRequest($"View/{path}", Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemListResponse> response = _client.Execute<ItemListResponse>(request);
            return response.Data;
        }

        public ItemListResponse GetAllSqlViewItems(string name)
        {
            var request = new RestRequest("View/sql/{name}", Method.GET);
            request.AddParameter("name", name, ParameterType.UrlSegment);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemListResponse> response = _client.Execute<ItemListResponse>(request);
            return response.Data;
        }

        public ItemResponse Reset()
        {
            var request = new RestRequest("reset", Method.PUT);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }

        public ItemResponse GetVersion()
        {
            var request = new RestRequest("../version", Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            IRestResponse<ItemResponse> response = _client.Execute<ItemResponse>(request);
            return response.Data;
        }
        public ItemListResponse MakeSearch(IDictionary<string, object> item, params string[] propertiesToRetrieve)
        {
            var request = new RestRequest("search", Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddQueryParameter("all", "true");
            request.AddJsonBody(item);
            if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            IRestResponse<ItemListResponse> response = _client.Execute<ItemListResponse>(request);
            return response.Data;
        }
    }
}
