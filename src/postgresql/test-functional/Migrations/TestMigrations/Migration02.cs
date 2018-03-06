using Dapper;
using RapidCore.Migration;
using RapidCore.PostgreSql.Migration;

namespace RapidCore.PostgreSql.FunctionalTests.Migrations.TestMigrations
{
    public class Migration02 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = ContextAs<PostgreSqlMigrationContext>().ConnectionProvider.Default();
            
            builder.Step("Add description column", async () =>
            {
                await db.ExecuteAsync(@"
                    alter table __Counter
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
