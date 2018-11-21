using System;
using DynamoApiClient.Models;

namespace DynamoApiClient
{
    public class ApiCallResponseException<T> : Exception where T : ApiResponse
    {
        public ApiCallResponseException(ApiCallResponse<T> response)
            :base(string.IsNullOrEmpty(response.ErrorMessage) ? response.Data.Error : response.ErrorMessage)
        {
            Response = response;
        }

        public ApiCallResponse<T> Response { get; set; }
    }
}