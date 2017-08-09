using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RapidCore.Mongo.FunctionalTests.Migration
{
    public class KewlEntity
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        
        public int Reference { get; set; }

        public static string Collection => "Kewl";
    }
}