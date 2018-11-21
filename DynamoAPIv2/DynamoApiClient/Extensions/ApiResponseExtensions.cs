using System;
using DynamoApiClient.Clients;
using DynamoApiClient.Models;

namespace DynamoApiClient.Extensions
{
    public static class ApiResponseExtensions
    {
        public static T ThrowIfErrorResponse<T>(this T response) where T : ApiResponse
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (!response.Success) throw new ApiResponseException(response);

            return response;
        }

        public static T ThrowIfErrorResponse<T>(this ApiCallResponse<T> response) where T : ApiResponse
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (!(response.IsSuccessful && response.Data.Success))
                throw new ApiCallResponseException<T>(response);

            return response.Data;
        }

        public static ItemsPage<ItemListResponse> AsPage(this ApiCallResponse<ItemListResponse> response, FluentClient client)
        {
            return AsPage(response, client.LowLevelClient);
        }

        public static ItemsPage<ItemListResponse> AsPage(this ApiCallResponse<ItemListResponse> response, Client client)
        {
            return new ItemsPage<ItemListResponse>(client, response);
        }

        public static ItemsPage<DynamoItemListResponse> AsPage(this ApiCallResponse<DynamoItemListResponse> response, FluentClient client)
        {
            return AsPage(response, client.LowLevelClient);
        }

        public static ItemsPage<DynamoItemListResponse> AsPage(this ApiCallResponse<DynamoItemListResponse> response, Client client)
        {
            return new ItemsPage<DynamoItemListResponse>(client, response);
        }
    }
}