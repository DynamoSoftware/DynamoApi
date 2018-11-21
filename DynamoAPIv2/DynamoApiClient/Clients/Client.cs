using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DynamoApiClient.Extensions;
using DynamoApiClient.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace DynamoApiClient.Clients
{
    public class Client
    {
        private class JsonNetDeserializer : IDeserializer
        {
            public JsonNetDeserializer()
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

        public bool DisableCache { get; set; }

        public static ApiCallResponse<TApiResponse> PlaceGetRequest<TApiResponse>(string apiUrl, string apiKey) where TApiResponse : ApiResponse, new()
        {
            var client = new RestClient(apiUrl);
            client.AddHandler("application/json", new JsonNetDeserializer());
            var request = new RestRequest("", Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            IRestResponse<TApiResponse> response = client.Execute<TApiResponse>(request);
            return response.ToApiCallResponse(client);
        }

        public Client(string apiKey, string apiUrl = "https://api.dynamosoftware.com/api/v2.0/")
        {
            _apiKey = apiKey;
            _client = new RestClient(apiUrl);
            _client.AddHandler("application/json", new JsonNetDeserializer());
        }

        internal ApiCallResponse<TApiResponse> PlaceRequest<TApiResponse>(string url,
            Method method = Method.GET,
            Action<RestRequest> modify = null)
            where TApiResponse : ApiResponse, new()
        {
            var request = new RestRequest(url, method);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");

            if (DisableCache)
            {
                request.AddHeader("Cache-Control", "no-cache");
            }

            modify?.Invoke(request);

            IRestResponse<TApiResponse> response = _client.Execute<TApiResponse>(request);
            return response.ToApiCallResponse(_client);
        }

        public ApiCallResponse<StringListResponse> GetEntities()
        {
            return PlaceRequest<StringListResponse>("Entity");
        }

        public ApiCallResponse<TotalResponse> GetAllEntityItemsCount(string entityName)
        {
            return PlaceRequest<TotalResponse>("Entity/{entityName}/total", modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            });
        }

        public ApiCallResponse<DynamoItemListResponse> GetEntityItems(string entityName, params string[] propertiesToRetrieve)
        {
            return PlaceRequest<DynamoItemListResponse>("Entity/{entityName}", modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
                if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                    request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            });
        }

        public ApiCallResponse<DynamoItemResponse> GetEntityItemById(string entityName, string id, params string[] propertiesToRetrieve)
        {
            return PlaceRequest<DynamoItemResponse>("Entity/{entityName}/{id}", modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
                request.AddParameter("id", id, ParameterType.UrlSegment);
                if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                    request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            });
        }

        public ApiCallResponse<DynamoItemListResponse> GetAllEntityItems(string entityName, params string[] propertiesToRetrieve)
        {
            return PlaceRequest<DynamoItemListResponse>("Entity/{entityName}", modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
                request.AddQueryParameter("all", "true");
                if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                    request.AddHeader("x-columns", String.Join(";", propertiesToRetrieve));
            });
        }

        public ApiCallResponse<ItemResponse> GetEntityProperties(string entityName)
        {
            return PlaceRequest<ItemResponse>("Entity/{entityName}/properties", modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            });
        }

        public ApiCallResponse<SchemaResponse> GetEntitySchema(string entityName)
        {
            return PlaceRequest<SchemaResponse>("Entity/{entityName}/schema", modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
            });
        }

        public ApiCallResponse<ItemResponse> DeleteEntityItem(string entityName, string id)
        {
            return PlaceRequest<ItemResponse>("Entity/{entityName}/{id}", Method.DELETE, modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
                request.AddParameter("id", id, ParameterType.UrlSegment);
            });
        }

        public ApiCallResponse<DynamoItemResponse> InsertEntityItem(string entityName, IDictionary<string, object> item)
        {
            return PlaceRequest<DynamoItemResponse>("Entity/{entityName}", Method.POST, modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
                request.AddJsonBody(item);
            });
        }

        public ApiCallResponse<DynamoItemResponse> UpdateEntityItem(string entityName, string id, IDictionary<string, object> item)
        {
            return PlaceRequest<DynamoItemResponse>("Entity/{entityName}/{id}", Method.PUT, modify: request =>
            {
                request.AddParameter("entityName", entityName, ParameterType.UrlSegment);
                request.AddParameter("id", id, ParameterType.UrlSegment);
                request.AddJsonBody(item);
            });
        }

        public ApiCallResponse<ItemListResponse> GetViews()
        {
            return PlaceRequest<ItemListResponse>("View");
        }

        public ApiCallResponse<DynamoItemListResponse> GetAllViewItems(string path)
        {
            return PlaceRequest<DynamoItemListResponse>($"View/{path}");
        }

        public ApiCallResponse<ItemListResponse> GetAllSqlViewItems(string name)
        {
            return PlaceRequest<ItemListResponse>("View/sql/{name}", modify: request =>
            {
                request.AddParameter("name", name, ParameterType.UrlSegment);
            });
        }

        public ApiCallResponse<ItemResponse> Reset()
        {
            return PlaceRequest<ItemResponse>("Reset", Method.PUT);
        }

        public ApiCallResponse<ItemResponse> GetVersion()
        {
            return PlaceRequest<ItemResponse>("../Version");
        }

        public ApiCallResponse<DynamoItemListResponse> MakeSearch(object advf, TimeSpan? utcOffset = null, bool all = false, params string[] propertiesToRetrieve)
        {
            return PlaceRequest<DynamoItemListResponse>("Search", Method.POST, modify: request =>
            {
                request.AddQueryParameter("all", all.ToString(CultureInfo.InvariantCulture));
                request.AddQueryParameter("utcOffset", utcOffset?.TotalHours.ToString(CultureInfo.InvariantCulture) ?? "0");

                if (!(advf is string))
                {
                    advf = JsonConvert.SerializeObject(advf);
                }

                request.AddParameter("application/json", advf, ParameterType.RequestBody);
                if (propertiesToRetrieve != null && propertiesToRetrieve.Any())
                    request.AddHeader("x-columns", string.Join(";", propertiesToRetrieve));
            });
        }
    }
}
