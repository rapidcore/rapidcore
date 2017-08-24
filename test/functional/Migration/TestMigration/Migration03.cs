using RapidCore.Migration;
using StackExchange.Redis;

namespace RapidCore.Redis.FunctionalTest.Migration.TestMigration
{
    public class Migration03 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = Context.Container.Resolve<IConnectionMultiplexer>().GetDatabase();
            
            builder.Step("Add thirteen", () =>
            {
                db.StringSetAsync("thirteen", "OMG OMG OMG");
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}