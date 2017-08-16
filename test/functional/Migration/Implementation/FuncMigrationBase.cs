using System.Threading.Tasks;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.Implementation
{
    /// <summary>
    /// Implementation for functional tests.
    /// 
    /// Uses the filesystem as storage.
    /// </summary>
    public abstract class FuncMigrationBase : MigrationBase
    {
        protected override async Task<MigrationInfo> GetMigrationInfoAsync()
        {
            var ctx = ContextAs<FuncMigrationContext>();

            return await Task.FromResult(ctx.Database.GetInfoByName(Name));
        }

        protected override async Task UpsertMigrationInfoAsync(MigrationInfo info)
        {
            var ctx = ContextAs<FuncMigrationContext>();
            ctx.Database.UpsertMigrationInfo(info);
            
            await Task.CompletedTask;
        }
    }
}