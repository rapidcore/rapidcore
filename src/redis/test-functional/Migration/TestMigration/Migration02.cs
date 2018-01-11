using RapidCore.Migration;
using StackExchange.Redis;

namespace RapidCore.Redis.FunctionalTest.Migration.TestMigration
{
    public class Migration02 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = Context.Container.Resolve<IConnectionMultiplexer>().GetDatabase();

            builder.Step("Update five", () =>
            {
                return db.StringSetAsync("five", "5 up");
            });
            
            builder.Step("Update seven", () =>
            {
                return db.StringSetAsync("seven", "7 up");
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}