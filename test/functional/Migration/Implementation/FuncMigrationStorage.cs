using System;
using System.Threading.Tasks;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.Implementation
{
    public class FuncMigrationStorage : IMigrationStorage
    {
        private FuncMigrationDatabase GetDatabase(IMigrationContext context)
        {
            return ((FuncMigrationContext) context).Database;
        }
        
        public async Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds)
        {
            var db = GetDatabase(context);
            
            var info = db.GetInfoByName(migration.Name);

            if (info == null)
            {
                info = new MigrationInfo
                {
                    Name = migration.Name
                };
            }
            
            info.CompletedAtUtc = DateTime.UtcNow;
            info.MigrationCompleted = true;
            info.TotalMigrationTimeInMs = milliseconds;
            
            db.UpsertMigrationInfo(info);
            await Task.CompletedTask;
        }

        public async Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName)
        {
            var db = GetDatabase(context);

            return await Task.FromResult(db.GetInfoByName(migrationName));
        }

        public async Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info)
        {
            var db = GetDatabase(context);
            
            db.UpsertMigrationInfo(info);
            
            await Task.CompletedTask;
        }

        public async Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName)
        {
            var db = GetDatabase(context);
            
            var info = db.GetInfoByName(migrationName);
            
            if (info == null)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(info.MigrationCompleted);
        }
    }
}