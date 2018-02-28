using Dapper;
using RapidCore.PostgreSql;
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
                    alter table __Counter
                    add column At timestamp
                    ;");
            });

            builder.Step("Add bla column", async () =>
            {
                await db.ExecuteAsync(@"
                    alter table __Counter
                    add column bla text
                    ;");
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
