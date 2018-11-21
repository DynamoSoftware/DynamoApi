using System.Reflection.Metadata.Ecma335;

namespace DynamoApiClient.Example
{
    public class ContactEntity
    {
        public const string Name = "Contact";

        public static class Properties
        {
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Email = "Email";
        }
    }
}