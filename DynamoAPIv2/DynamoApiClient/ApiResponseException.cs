using System;
using DynamoApiClient.Models;

namespace DynamoApiClient
{
    public class ApiResponseException : Exception
    {
        public ApiResponseException(ApiResponse response)
            : base(response.Error)
        {
            Response = response;
        }

        public ApiResponse Response { get; set; }
    }
}