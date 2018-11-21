using System;
using System.Linq;
using DynamoApiClient.Models;
using RestSharp;

namespace DynamoApiClient.Extensions
{
    public static class RestResponseExtensions
    {
        internal static ApiCallResponse<T> ToApiCallResponse<T>(this IRestResponse<T> response, RestClient client = null)
            where T : ApiResponse
        {
            return new ApiCallResponse<T>
            {
                Content = response.Content,
                ContentEncoding = response.ContentEncoding,
                ContentLength = response.ContentLength,
                ContentType = response.ContentType,
                Cookies = response.Cookies.Select(c=>new System.Net.Cookie(c.Name, c.Value)
                {
                    Domain = c.Domain,
                    Expires = c.Expires,
                    HttpOnly = c.HttpOnly,
                    Path = c.Path,
                    Secure = c.Secure,
                }).ToList(),
                ErrorException = response.ErrorException,
                ErrorMessage = response.ErrorMessage,
                Headers = response.Headers.ToDictionary(k=>k.Name, v=>v.Value.ToString()),
                ProtocolVersion = response.ProtocolVersion,
                Request = response.Request == null 
                    ? null 
                    : new ApiCallRequest { Method = Enum.GetName(typeof(Method), response.Request.Method), Uri = client?.BuildUri(response.Request)},
                ResponseUri = response.ResponseUri,
                Server = response.Server,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
                Data = response.Data,
                IsSuccessful = response.IsSuccessful,
            };
        }
    }
}