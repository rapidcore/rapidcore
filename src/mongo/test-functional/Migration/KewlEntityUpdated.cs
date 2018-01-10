using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RapidCore.Mongo.FunctionalTests.Migration
{
    public class KewlEntityUpdated
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
            
        public string Reference { get; set; }
            
        public string Mucho { get; set; }
            
        public static string Collection => "Kewl";
    }
}