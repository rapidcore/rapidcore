using Dapper;
using Rapidcore.Postgresql;
using RapidCore.Migration;

namespace functionaltests.Migrations.TestMigrations
{
    public class Migration01 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = ContextAs<PostgreSqlMigrationContext>().ConnectionProvider.Default();
            
            builder.Step("Add at column", async () =>
            {
                await db.ExecuteAsync(@"
                    alter table Counter
                    add column At timestamp
                    ;");
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
