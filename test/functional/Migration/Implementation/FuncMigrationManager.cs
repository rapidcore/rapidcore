using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.Implementation
{
    public class FuncMigrationManager : ReflectionMigrationManagerBase
    {
        public FuncMigrationManager(IList<Assembly> assemblies) : base(assemblies)
        {
        }

        public FuncMigrationManager(Assembly assembly) : base(assembly)
        {
        }

        public override async Task MarkAsCompleteAsync(IMigration migration, long milliseconds, IMigrationContext context)
        {
            var db = ((FuncMigrationContext) context).Database;

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

        protected override async Task<bool> HasMigrationBeenFullyCompletedAsync(string migrationName, IMigrationContext context)
        {
            var db = ((FuncMigrationContext) context).Database;

            var info = db.GetInfoByName(migrationName);
            
            if (info == null)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(info.MigrationCompleted);
        }
    }
}