using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Dynamo.Api.Tests
{
    public class DynamoV1Client
    {
        private readonly DynamoApiContext _context;

        private DynamoV1Client(DynamoApiContext context)
        {
            _context = context;
        }

        public static DynamoV1Client Login(Uri dynamoUrl, string userName, string password, string tenant)
        {
            var context = new DynamoApiContext(dynamoUrl);

            // Prepare url
            var url = new Uri(context.DynamoApiUrl, "Login");

            WebClient client = SetUpWebClient(context, url);

            var payload = new LoginRequest { UserName = userName, Password = password, Tenant = tenant };

            client.UploadString(url, HttpMethod.Post.Method, JsonConvert.SerializeObject(payload));

            context.CookieContainer.SetCookies(url, client.ResponseHeaders[HttpResponseHeader.SetCookie]);

            return new DynamoV1Client(context);
        }

        public SearchDocumentResponse SearchDocuments(string query)
        {
            // Prepare url
            var urlBuilder = new UriBuilder(new Uri(_context.DynamoApiUrl, "SearchDocuments"));
            urlBuilder.Query = String.Concat("query=", Uri.EscapeUriString(query));
            Uri url = urlBuilder.Uri;

            WebClient client = SetUpWebClient(_context, url);

            string responseRaw = client.DownloadString(url);

            return JsonConvert.DeserializeObject<SearchDocumentResponse>(responseRaw);
        }

        public IEnumerable<SearchDocumentResponseItem> SearchDocumentsContinuous(string query)
        {
            string internalQuery = query;

            do
            {
                var response = SearchDocuments(internalQuery);
                foreach (var item in response.Items)
                {
                    yield return item;
                }
                internalQuery = response.NextToken;
            } while (internalQuery != null);
        }

        public void GetDocument(string saveDirectory, string documentId)
        {
            if (!Directory.Exists(saveDirectory))
                throw new DirectoryNotFoundException();

            // Prepare url
            var urlBuilder = new UriBuilder(new Uri(_context.DynamoApiUrl, "GetDocument"));
            urlBuilder.Query = String.Concat("id=", Uri.EscapeUriString(documentId));
            Uri url = urlBuilder.Uri;

            WebClient client = SetUpWebClient(_context, url);

            string tempFileName = Path.Combine(saveDirectory, documentId);
            client.DownloadFile(url, tempFileName);

            string cpString = client.ResponseHeaders["Content-Disposition"];
            ContentDisposition contentDisposition = new ContentDisposition(cpString);
            string filename = contentDisposition.FileName;
            File.Move(tempFileName, Path.Combine(saveDirectory, filename));
        }

        public void GetDocuments(string saveDirectory, params string[] documentIds)
        {
            if (!Directory.Exists(saveDirectory))
                throw new DirectoryNotFoundException();

            if (documentIds == null || documentIds.Length == 0)
                throw new ArgumentNullException("documentIds");

            // Prepare url
            string docIdsParam = String.Join(";", documentIds);
            var urlBuilder = new UriBuilder(new Uri(_context.DynamoApiUrl, "GetDocuments"));
            urlBuilder.Query = String.Concat("docIds=", Uri.EscapeUriString(docIdsParam));
            Uri url = urlBuilder.Uri;

            WebClient client = SetUpWebClient(_context, url);

            string tempFileName = Path.Combine(saveDirectory, Guid.NewGuid().ToString("D"));
            client.DownloadFile(url, tempFileName);

            string cpString = client.ResponseHeaders["Content-Disposition"];
            ContentDisposition contentDisposition = new ContentDisposition(cpString);
            string filename = contentDisposition.FileName;
            File.Move(tempFileName, Path.Combine(saveDirectory, filename));
        }

        private static WebClient SetUpWebClient(DynamoApiContext context, Uri url)
        {
            // Setup web client
            string cookieHeaderValue = context.CookieContainer.GetCookieHeader(url);
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Cookie, cookieHeaderValue);
            client.Headers.Add(HttpRequestHeader.Accept, "application/json");
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            return client;
        }
    }
}