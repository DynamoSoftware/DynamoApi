using System;
using DynamoApiClient.Models;

namespace DynamoApiClient.Extensions
{
    public static class ApiResponseExtensions
    {
        public static T ThrowIfErrorResponse<T>(this T response) where T : ApiResponse
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (!response.Success) throw new Exception(response.Error);

            return response;
        }
    }
}