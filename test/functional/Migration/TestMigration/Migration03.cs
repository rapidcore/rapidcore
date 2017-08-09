using MongoDB.Driver;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;

namespace RapidCore.Mongo.FunctionalTests.Migration.TestMigration
{
    public class Migration03 : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = Context.ConnectionProvider.Default();
            
            builder.Step("Add 'Mucho' to 'five'", async () =>
            {
                var filter = Builders<KewlEntityUpdated>.Filter.Eq(x => x.Reference, "5");
                
                var update = Builders<KewlEntityUpdated>.Update
                    .Set("Mucho", "Ulla Henriksen");

                await db.GetCollection<KewlEntityUpdated>(KewlEntityUpdated.Collection).UpdateManyAsync(filter, update);
            });
            
            builder.Step("Add 'Mucho' to 'seven'", async () =>
            {
                var filter = Builders<KewlEntityUpdated>.Filter.Eq(x => x.Reference, "7");
                
                var update = Builders<KewlEntityUpdated>.Update
                    .Set("Mucho", "Bubbly");

                await db.GetCollection<KewlEntityUpdated>(KewlEntityUpdated.Collection).UpdateManyAsync(filter, update);
            });
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}