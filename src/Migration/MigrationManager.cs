using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RapidCore.Mongo.Migration.Internal;
using ServiceStack;

namespace RapidCore.Mongo.Migration
{
    public class MigrationManager : IMigrationManager
    {
        private readonly IList<Assembly> assemblies;

        public MigrationManager(IList<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        #region FindMigrationsForUpgradeAsync
        public async Task<IList<IMigration>> FindMigrationsForUpgradeAsync(MigrationContext context)
        {
            var migrations = new SortedList<string, IMigration>();

            foreach (var asm in assemblies)
            {
                var types = asm.ExportedTypes
                    .Where(x => x.HasInterface(typeof(IMigration)))
                    .Select(x => x);

                foreach (var type in types)
                {
                    var instance = GetMigrationInstance(type, context);

                    var completedDoc = await context.ConnectionProvider.Default()
                        .FirstOrDefaultAsync<MigrationDocument>(MigrationDocument.CollectionName,
                            x => x.Name == instance.Name && x.MigrationCompleted);

                    if (completedDoc == null)
                    {
                        migrations.Add(instance.Name, instance);
                    }
                }
            }

            return migrations.Values;
        }

        private IMigration GetMigrationInstance(Type type, MigrationContext context)
        {
            var x = context.Container.Resolve(type);

            if (x != null)
            {
                return (IMigration)x;
            }

            return (IMigration)Activator.CreateInstance(type);
        }
        #endregion
        
        #region Mark as complete
        public async Task MarkAsCompleteAsync(IMigration migration, long milliseconds, MigrationContext context)
        {
            var db = context.ConnectionProvider.Default();

            var doc = await db.FirstOrDefaultAsync<MigrationDocument>(MigrationDocument.CollectionName, x => x.Name == migration.Name);

            if (doc == default(MigrationDocument))
            {
                doc = new MigrationDocument
                {
                    Name = migration.Name
                };
            }
            
            doc.CompletedAt = DateTimeOffset.UtcNow;
            doc.MigrationCompleted = true;
            doc.TotalMigrationTimeInMs = milliseconds;

            await db.UpsertAsync<MigrationDocument>(MigrationDocument.CollectionName, doc, x => x.Name == doc.Name);
        }
        #endregion
    }
}