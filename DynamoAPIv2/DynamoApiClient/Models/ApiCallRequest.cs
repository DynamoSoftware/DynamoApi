using System;

namespace DynamoApiClient.Models
{
    public class ApiCallRequest
    {
        /// <summary>
        /// Determines what HTTP method to use for this request. Supported methods: GET, POST, PUT, DELETE, HEAD, OPTIONS
        /// </summary>
        public string Method { get; set; }

        public Uri Uri { get; set; }
    }
}