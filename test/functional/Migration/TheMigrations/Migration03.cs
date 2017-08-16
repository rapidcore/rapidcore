using System.Threading.Tasks;
using RapidCore.FunctionalTests.Migration.Implementation;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.TheMigrations
{
    public class Migration03 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = ContextAs<FuncMigrationContext>().Database;
            
            builder.Step("Add 'Mucho' to 'five'", async () =>
            {
                var kewl = db.GetKewlById("five");
                kewl.Reference = $"Mucho {kewl.Reference}";
                
                db.UpsertKewl(kewl);
                await Task.CompletedTask;
            });
            
            builder.Step("Add 'Mucho' to 'seven'", async () =>
            {
                var kewl = db.GetKewlById("seven");
                kewl.Reference = $"Mucho {kewl.Reference}";
                
                db.UpsertKewl(kewl);
                await Task.CompletedTask;
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}