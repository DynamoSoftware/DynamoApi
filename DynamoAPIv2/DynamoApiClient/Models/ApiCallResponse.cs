using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace DynamoApiClient.Models
{
    public class ApiCallResponse<T> where T : ApiResponse
    {
        public T Data { get; set; }

        /// <summary>
        /// The RestRequest that was made to get this RestResponse
        /// </summary>
        /// <remarks>Mainly for debugging if ResponseStatus is not OK</remarks>
        public ApiCallRequest Request { get; set; }

        /// <summary>MIME content type of response</summary>
        public string ContentType { get; set; }

        /// <summary>Length in bytes of the response content</summary>
        public long ContentLength { get; set; }

        /// <summary>Encoding of the response content</summary>
        public string ContentEncoding { get; set; }

        /// <summary>String representation of response content</summary>
        public string Content { get; set; }

        /// <summary>HTTP response status code</summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Whether or not the response status code indicates success
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>Description of HTTP status returned</summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// The URL that actually responded to the content (different from request if redirected)
        /// </summary>
        public Uri ResponseUri { get; set; }

        /// <summary>HttpWebResponse.Server</summary>
        public string Server { get; set; }

        /// <summary>Cookies returned by server with the response</summary>
        public IList<Cookie> Cookies { get; set; }

        /// <summary>Headers returned by server with the response</summary>
        public Dictionary<string, string> Headers { get; set; }

        /*        /// <summary>
                /// Status of the request. Will return Error for transport errors.
                /// HTTP errors will still return ResponseStatus.Completed, check StatusCode instead
                /// </summary>
                ResponseStatus ResponseStatus { get; set; }*/

        /// <summary>
        /// Transport or other non-HTTP error generated while attempting request
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>Exceptions thrown during the request, if any.</summary>
        /// <remarks>Will contain only network transport or framework exceptions thrown during the request.
        /// HTTP protocol errors are handled by RestSharp and will not appear here.</remarks>
        public Exception ErrorException { get; set; }

        /// <summary>The HTTP protocol version (1.0, 1.1, etc)</summary>
        /// <remarks>Only set when underlying framework supports it.</remarks>
        public Version ProtocolVersion { get; set; }
    }
}