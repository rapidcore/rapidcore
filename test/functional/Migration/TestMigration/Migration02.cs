using MongoDB.Driver;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;

namespace RapidCore.Mongo.FunctionalTests.Migration.TestMigration
{
    public class Migration02 : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = Context.ConnectionProvider.Default();
            
            builder.Step("Change Reference from int to string", () =>
            {
                var collection = db.GetCollection<MigrationTests.KewlEntity>(MigrationTests.KewlEntity.Collection);

                var cursor = collection
                    .Find(_ => true)
                    .ToCursor();

                cursor.ForEachAsync(doc =>
                {
                    var filter = Builders<MigrationTests.KewlEntityUpdated>.Filter.Eq(x => x.Id, doc.Id);
                    var update = Builders<MigrationTests.KewlEntityUpdated>.Update.Set("Reference", doc.Reference.ToString());

                    db.GetCollection<MigrationTests.KewlEntityUpdated>(MigrationTests.KewlEntityUpdated.Collection).UpdateMany(filter, update);
                });
            });
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}