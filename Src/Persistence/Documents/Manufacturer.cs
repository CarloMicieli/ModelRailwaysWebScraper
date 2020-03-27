using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.Documents
{
    public class Manufacturer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Source Source { get; set; }

        public string Name { get; set; }
    }
}