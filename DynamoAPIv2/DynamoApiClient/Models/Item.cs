using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DynamoApiClient.Models
{
    public class Item : Dictionary<string, object>
    {
        public Item()
        {
        }

        public Item(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Item(int capacity) : base(capacity)
        {
        }

        public Item(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        public Item(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public Item(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        public Item(IDictionary<string, object> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        {
        }

        public bool TryGetProperty<T>(string name, out T value)
        {
            if (ContainsKey(name) && this[name] is T propertyValue)
            {
                value = propertyValue;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}