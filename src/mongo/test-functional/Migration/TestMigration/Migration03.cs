using MongoDB.Driver;
using RapidCore.Migration;
using RapidCore.Mongo.Migration;

namespace RapidCore.Mongo.FunctionalTests.Migration.TestMigration
{
    public class Migration03 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = ContextAs<MongoMigrationContext>().ConnectionProvider.Default();
            
            builder.Step("Add 'Mucho' to 'five'", async () =>
            {
                var filter = Builders<KewlEntityUpdated>.Filter.Eq(x => x.Reference, "5");
                
                var update = Builders<KewlEntityUpdated>.Update
                    .Set("Mucho", "Ulla Henriksen");

                await db.GetCollection<KewlEntityUpdated>().UpdateManyAsync(filter, update);
            });
            
            builder.Step("Add 'Mucho' to 'seven'", async () =>
            {
                var filter = Builders<KewlEntityUpdated>.Filter.Eq(x => x.Reference, "7");
                
                var update = Builders<KewlEntityUpdated>.Update
                    .Set("Mucho", "Bubbly");

                await db.GetCollection<KewlEntityUpdated>().UpdateManyAsync(filter, update);
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}