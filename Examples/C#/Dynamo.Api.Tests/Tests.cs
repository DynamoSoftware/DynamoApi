using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dynamo.Api.Tests
{
    [TestClass]
    public class Tests
    {
        private static readonly Uri DynamoUri = new Uri("http://localhost:65318"); // https://dynamoweb1.netagesolutions.com:6070
        private const string UserName = "dynamoadmin@mirilpevc.com"; // dynamoadmin@skkpevc2.com
        private const string Password = "mirilpevc.2017"; // skkpevc2.2016
        private const string Tenant = "mirilpevc"; // skkpevc2

        [TestMethod]
        [TestCategory("Login")]
        public void InvalidLogin()
        {
            try
            {
                DynamoV1Client.Login(DynamoUri, "Invalid User Name", "Invalid Password", Tenant);
                Assert.Fail();
            }
            catch (WebException)
            {
            }
        }

        [TestMethod]
        [TestCategory("Login")]
        public void ValidLogin()
        {
            DynamoV1Client.Login(DynamoUri, UserName, Password, Tenant);
        }

        [TestMethod]
        [TestCategory("SearchDocument")]
        public void SearchDocumentNextToken()
        {
            var client = DynamoV1Client.Login(DynamoUri, UserName, Password, Tenant);
            var documentResults =
                client.SearchDocuments("friday OR letter OR event OR fund");

            Assert.IsNotNull(documentResults.NextToken);
            Assert.AreEqual(10, documentResults.Items.Length);
        }

        [TestMethod]
        [TestCategory("SearchDocument")]
        public void SearchDocument()
        {
            var client = DynamoV1Client.Login(DynamoUri, UserName, Password, Tenant);
            SearchDocumentResponseItem[] documentResults =
                client.SearchDocumentsContinuous("friday OR letter OR event OR fund").Take(20).ToArray();

            Assert.AreEqual(13, documentResults.Length);
        }
    }
}
