using System;
using System.Collections.Generic;
using System.Linq;
using DynamoApiClient.Clients;
using DynamoApiClient.Endpoints;
using DynamoApiClient.Models;
using Newtonsoft.Json;

namespace DynamoApiClient.Example
{
    public class Program
    {
        static void Main(string[] args)
        {
            FluentClient client = new FluentClient(
                "Paste API KEY here!",
                "https://api.dynamosoftware.com/api/v2.0/");

            EntityEndpoint contactEntity = client.Entities[ContactEntity.Name];
  
            List<DynamoItem> contactCollection = contactEntity.Items.Take(50).ToList();
            IDictionary<string, Type> contactProperties = contactEntity.Properties;
            SchemaResponse.EntitySchema contactSchema = contactEntity.Schema;

            var contactToInsert = new Dictionary<string, object>
            {
                { ContactEntity.Properties.FirstName, "John"},
                { ContactEntity.Properties.LastName, "Doe"},
                { ContactEntity.Properties.Email, "example@email.com"}
            };

            // Creates a contact and returns it
            DynamoItem contact = contactEntity
                .Insert(contactToInsert);

            var updateData = new Dictionary<string, object>
            {
                { ContactEntity.Properties.Email, "modified.Example@email.com"}
            };

            // Updates the contact and returns the updated properties
            DynamoItem updatedContact = contactEntity
                .Update(contact.Id, updateData);

            // Retrieves the contact by id
            DynamoItem retrievedContact = contactEntity
                .ForProperties(ContactEntity.Properties.FirstName, 
                    ContactEntity.Properties.LastName,
                    ContactEntity.Properties.Email)
                .GetById(contact.Id);

            // Deletes the updated contact
            contactEntity.Delete(updatedContact.Id);

            //Retrieve all Contacts with first name sounding like John
            //Query JSON can be taken from Advanced find's `API Query` button in Dynamo
            var query = JsonConvert.DeserializeObject<object>(@"
                    {
                        ""advf"": {
                            ""e"": [{
                                ""_name"": ""Contact"",
                                ""rule"": [{
                                    ""_op"": ""soundex"",
                                    ""_prop"": ""First name"",
                                    ""values"": [
                                        ""John""
                                    ]
                                }]
                            }]
                        }
                    }");

            var first50Johns = client.Search(query)
                .ForProperties(ContactEntity.Properties.FirstName,
                    ContactEntity.Properties.LastName,
                    ContactEntity.Properties.Email)
                .Items
                .Take(50)
                .ToList();
        }
    }
}
