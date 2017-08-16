using RapidCore.FunctionalTests.Migration.Implementation;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.TheMigrations
{
    public class Migration02 : FuncMigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            var db = ContextAs<FuncMigrationContext>().Database;
            
            builder.Step("Add 'yay' to reference", () =>
            {
                foreach (var kewl in db.AllKewl())
                {
                    kewl.Reference = $"{kewl.Reference} yay";
                    db.UpsertKewl(kewl);
                }
            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}