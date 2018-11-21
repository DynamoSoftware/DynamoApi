using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class DynamoItem : Item
    {
        public DynamoItem()
        {
        }

        public DynamoItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DynamoItem(int capacity) : base(capacity)
        {
        }

        public DynamoItem(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        public DynamoItem(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public DynamoItem(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        public DynamoItem(IDictionary<string, object> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        {
        }

        [JsonIgnore]
        public string Id
        {
            get => this["_id"] as string;
            set => this["_id"] = value;
        }

        [JsonIgnore]
        public string EntitySchemaName
        {
            get => this["_es"] as string;
            set => this["_es"] = value;
        }
    }
}