using System;
using Newtonsoft.Json;

namespace DynamoApiClient.Models
{
    public class SchemaResponse : TypedResponse<SchemaResponse.EntitySchema>
    {
        public class EntitySchema : IEquatable<EntitySchema>
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("identity")]
            public string Identity { get; set; }

            [JsonProperty("properties")]
            public PropertySchema[] Properties { get; set; }

            [JsonProperty("references")]
            public EntityReference[] References { get; set; }

            public bool Equals(EntitySchema other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && string.Equals(Label, other.Label) &&
                       string.Equals(Identity, other.Identity) && Equals(Properties, other.Properties) &&
                       Equals(References, other.References);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((EntitySchema)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (Name != null ? Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Label != null ? Label.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Identity != null ? Identity.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Properties != null ? Properties.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (References != null ? References.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(EntitySchema left, EntitySchema right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(EntitySchema left, EntitySchema right)
            {
                return !Equals(left, right);
            }
        }

        public class PropertySchema : IEquatable<PropertySchema>
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("type")]
            public PropertySchemaType Type { get; set; }

            [JsonProperty("readOnly", NullValueHandling = NullValueHandling.Ignore)]
            public bool? ReadOnly { get; set; }

            /*[JsonProperty("allowedEntities", NullValueHandling = NullValueHandling.Ignore)]
            public string AllowedEntities { get; set; }*/

            [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
            public string Description { get; set; }

            public bool Equals(PropertySchema other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && string.Equals(Label, other.Label) &&
                       Equals(Type, other.Type) && ReadOnly == other.ReadOnly &&
                       string.Equals(Description, other.Description);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PropertySchema)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (Name != null ? Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Label != null ? Label.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ ReadOnly.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(PropertySchema left, PropertySchema right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PropertySchema left, PropertySchema right)
            {
                return !Equals(left, right);
            }
        }

        public class PropertySchemaType : IEquatable<PropertySchemaType>
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("isRequired")]
            public bool IsRequired { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public long? Size { get; set; }

            [JsonProperty("showTime", NullValueHandling = NullValueHandling.Ignore)]
            public bool? ShowTime { get; set; }

            [JsonProperty("isHtml", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsHtml { get; set; }

            [JsonProperty("isIPAddress", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsIpAddress { get; set; }

            [JsonProperty("imageSource", NullValueHandling = NullValueHandling.Ignore)]
            public string ImageSource { get; set; }

            [JsonProperty("separator", NullValueHandling = NullValueHandling.Ignore)]
            public string Separator { get; set; }

            [JsonProperty("references", NullValueHandling = NullValueHandling.Ignore)]
            public PropertySchemaTypeReference[] References { get; set; }

            [JsonProperty("isReference", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsReference { get; set; }

            [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
            public string Alias { get; set; }

            [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
            public string Format { get; set; }

            public bool Equals(PropertySchemaTypeReference[] left, PropertySchemaTypeReference[] right)
            {
                if (left == null && right == null) return true;
                if (left == null || right == null || left.Length != right.Length) return false;
                foreach (var entityReference in left)
                {
                    bool contains = false;
                    foreach (var reference in right)
                    {
                        if (entityReference == reference)
                            contains = true;
                    }

                    if (contains == false)
                        return false;
                }

                return true;
            }

            public bool Equals(PropertySchemaType other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && IsRequired == other.IsRequired && Size == other.Size &&
                       ShowTime == other.ShowTime && IsHtml == other.IsHtml && IsIpAddress == other.IsIpAddress &&
                       string.Equals(ImageSource, other.ImageSource) && string.Equals(Separator, other.Separator) &&
                       Equals(References, other.References) && IsReference == other.IsReference &&
                       string.Equals(Alias, other.Alias) && string.Equals(Format, other.Format);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PropertySchemaType)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (Name != null ? Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ IsRequired.GetHashCode();
                    hashCode = (hashCode * 397) ^ Size.GetHashCode();
                    hashCode = (hashCode * 397) ^ ShowTime.GetHashCode();
                    hashCode = (hashCode * 397) ^ IsHtml.GetHashCode();
                    hashCode = (hashCode * 397) ^ IsIpAddress.GetHashCode();
                    hashCode = (hashCode * 397) ^ (ImageSource != null ? ImageSource.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Separator != null ? Separator.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (References != null ? References.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ IsReference.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Alias != null ? Alias.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Format != null ? Format.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(PropertySchemaType left, PropertySchemaType right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PropertySchemaType left, PropertySchemaType right)
            {
                return !Equals(left, right);
            }
        }

        public class PropertySchemaTypeReference : IEquatable<PropertySchemaTypeReference>
        {
            [JsonProperty("referenceName", NullValueHandling = NullValueHandling.Ignore)]
            public string ReferenceName { get; set; }

            [JsonProperty("referenceProp", NullValueHandling = NullValueHandling.Ignore)]
            public string ReferenceProp { get; set; }

            [JsonProperty("referenceEntity", NullValueHandling = NullValueHandling.Ignore)]
            public string ReferenceEntity { get; set; }

            public bool Equals(PropertySchemaTypeReference other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(ReferenceName, other.ReferenceName) && string.Equals(ReferenceProp, other.ReferenceProp) && string.Equals(ReferenceEntity, other.ReferenceEntity);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PropertySchemaTypeReference)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (ReferenceName != null ? ReferenceName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (ReferenceProp != null ? ReferenceProp.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (ReferenceEntity != null ? ReferenceEntity.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(PropertySchemaTypeReference left, PropertySchemaTypeReference right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PropertySchemaTypeReference left, PropertySchemaTypeReference right)
            {
                return !Equals(left, right);
            }
        }

        public class EntityReference : IEquatable<EntityReference>
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("entity")]
            public string Entity { get; set; }

            public bool Equals(EntityReference other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && string.Equals(Entity, other.Entity);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((EntityReference)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Entity != null ? Entity.GetHashCode() : 0);
                }
            }

            public static bool operator ==(EntityReference left, EntityReference right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(EntityReference left, EntityReference right)
            {
                return !Equals(left, right);
            }
        }
    }
}