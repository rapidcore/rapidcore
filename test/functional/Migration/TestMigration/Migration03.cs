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
            
            builder.WithStep("Add 'Mucho' to 'five'", () =>
            {
                var filter = Builders<MigrationTests.KewlEntityUpdated>.Filter.Eq(x => x.Reference, "5");
                
                var update = Builders<MigrationTests.KewlEntityUpdated>.Update
                    .Set("Mucho", "Ulla Henriksen");

                db.GetCollection<MigrationTests.KewlEntityUpdated>(MigrationTests.KewlEntityUpdated.Collection).UpdateMany(filter, update);
            });
            
            builder.WithStep("Add 'Mucho' to 'seven'", () =>
            {
                var filter = Builders<MigrationTests.KewlEntityUpdated>.Filter.Eq(x => x.Reference, "7");
                
                var update = Builders<MigrationTests.KewlEntityUpdated>.Update
                    .Set("Mucho", "Bubbly");

                db.GetCollection<MigrationTests.KewlEntityUpdated>(MigrationTests.KewlEntityUpdated.Collection).UpdateMany(filter, update);
            });
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}