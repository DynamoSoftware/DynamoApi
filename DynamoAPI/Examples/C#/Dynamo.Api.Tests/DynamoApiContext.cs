using System;
using System.Net;

namespace Dynamo.Api.Tests
{
    internal class DynamoApiContext
    {
        public DynamoApiContext(Uri dynamoUrl)
        {
            DynamoUrl = dynamoUrl;
            DynamoApiUrl = new Uri(dynamoUrl, "/new/v1/");
            CookieContainer = new CookieContainer();
        }

        public Uri DynamoUrl { get; private set; }

        public Uri DynamoApiUrl { get; private set; }

        public CookieContainer CookieContainer { get; private set; }
    }
}