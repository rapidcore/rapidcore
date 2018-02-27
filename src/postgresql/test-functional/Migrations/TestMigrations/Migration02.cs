using Dapper;
using Rapidcore.Postgresql;
using RapidCore.Migration;

namespace functionaltests.Migrations.TestMigrations
{
    public class Migration02 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = ContextAs<PostgreSqlMigrationContext>().ConnectionProvider.Default();
            
            builder.Step("Add description column", async () =>
            {
                await db.ExecuteAsync(@"
                    alter table Counter
                    add column Description text DEFAULT 'sample default value'
                    ;");
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
