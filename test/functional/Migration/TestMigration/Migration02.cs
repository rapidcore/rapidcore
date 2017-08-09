using MongoDB.Driver;
using RapidCore.Mongo.Migration;

namespace RapidCore.Mongo.FunctionalTests.Migration.TestMigration
{
    public class Migration02 : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = Context.ConnectionProvider.Default();
            
            builder.Step("Change Reference from int to string", () =>
            {
                var collection = db.GetCollection<KewlEntity>(KewlEntity.Collection);

                var cursor = collection
                    .Find(_ => true)
                    .ToCursor();

                return cursor.ForEachAsync(doc =>
                {
                    var filter = Builders<KewlEntityUpdated>.Filter.Eq(x => x.Id, doc.Id);
                    var update = Builders<KewlEntityUpdated>.Update.Set("Reference", doc.Reference.ToString());

                    db.GetCollection<KewlEntityUpdated>(KewlEntityUpdated.Collection).UpdateMany(filter, update);
                });
            });
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}