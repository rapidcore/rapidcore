using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;

namespace RapidCore.Mongo.FunctionalTests.Migration.TestMigration
{
    public class Migration01 : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}