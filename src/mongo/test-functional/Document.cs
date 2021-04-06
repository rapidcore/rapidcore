using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RapidCore.Mongo.FunctionalTests
{
    [Entity(CollectionName = "documents")]
    public class Document
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string String { get; set; }

        public string Aux { get; set; }
    }
}
