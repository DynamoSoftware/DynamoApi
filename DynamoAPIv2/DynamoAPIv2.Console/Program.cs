using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoApiClient.Clients;

namespace DynamoAPIv2.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Examples.");

            FluentClient fluentClient = new FluentClient("Paste API Key here.",
                "https://api.dynamosoftware.com/api/v2.0/");

            var contactCollection = fluentClient.Entities["Contact"].Items;
            var contactProperties = fluentClient.Entities["Contact"].Properties;
            var contactSchema = fluentClient.Entities["Contact"].Schema;

            var contactToInsert = new Dictionary<string, object>
            {
                { "FirstName", "John"},
                { "LastName", "Doe"},
                { "Email", "example@email.com"}
            };

            // Creates a contact and returns it
            var contact = fluentClient.Entities["Contact"]
                .ForProperties("FirstName", "LastName", "Email")
                .Insert(contactToInsert);

            var columnsToUpdate = new Dictionary<string, object>
            {
                { "Email", "modified.Example@email.com"}
            };

            // Updates the contact and returns the updated properties
            var updatedContact = fluentClient.Entities["Contact"].Update(contact["_id"].ToString(), columnsToUpdate);


            // Deletes the updated contact
            fluentClient.Entities["Contact"].Delete(updatedContact["_id"].ToString());
        }
    }
}
