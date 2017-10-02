using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using RapidCore.Migration;
using RapidCore.Mongo.Migration.Internal;

namespace RapidCore.Mongo.Migration
{
    public class MongoMigrationStorage : IMigrationStorage
    {
        private MongoDbConnection GetDb(IMigrationContext context)
        {
            return ((MongoMigrationContext) context).ConnectionProvider.Default();
        }
        
        public async Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds)
        {
            var db = GetDb(context);

            var doc = await db.FirstOrDefaultAsync<MigrationDocument>(MigrationDocument.CollectionName, x => x.Name == migration.Name);

            if (doc == default(MigrationDocument))
            {
                doc = new MigrationDocument
                {
                    Name = migration.Name
                };
            }
            
            doc.CompletedAtUtc = DateTime.UtcNow;
            doc.MigrationCompleted = true;
            doc.TotalMigrationTimeInMs = milliseconds;

            await db.UpsertAsync<MigrationDocument>(MigrationDocument.CollectionName, doc, x => x.Name == doc.Name);
        }

        public async Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName)
        {
            var db = GetDb(context);
            
            var doc = await db
                .FirstOrDefaultAsync<MigrationDocument>
                (
                    MigrationDocument.CollectionName,
                    document => document.Name == migrationName
                );

            if (doc == default(MigrationDocument))
            {
                return default(MigrationInfo);
            }

            return ToMigrationInfo(doc);
        }

        public async Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info)
        {
            var db = GetDb(context);

            var doc = ToMigrationDocument(info);

            await db.UpsertAsync(MigrationDocument.CollectionName, doc, x => x.Name == info.Name);
        }

        public async Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName)
        {
            var db = GetDb(context);
            
            var completedDoc = await db.FirstOrDefaultAsync<MigrationDocument>
            (
                MigrationDocument.CollectionName,
                x => x.Name == migrationName && x.MigrationCompleted
            );

            return completedDoc != default(MigrationDocument);
        }

        #region Mapping
        protected MigrationInfo ToMigrationInfo(MigrationDocument doc)
        {
            var id = string.Empty;

            if (doc.Id != default(ObjectId))
            {
                id = doc.Id.ToString();
            }
            
            return new MigrationInfo
            {
                CompletedAtUtc = doc.CompletedAtUtc,
                Id = id,
                MigrationCompleted = doc.MigrationCompleted,
                Name = doc.Name,
                StepsCompleted = doc.StepsCompleted,
                TotalMigrationTimeInMs = doc.TotalMigrationTimeInMs
            };
        }

        protected MigrationDocument ToMigrationDocument(MigrationInfo info)
        {
            var oid = ObjectId.Empty;
            if (!string.IsNullOrEmpty(info.Id))
            {
                oid = ObjectId.Parse(info.Id);
            }
            
            return new MigrationDocument
            {
                CompletedAtUtc = info.CompletedAtUtc,
                Id = oid,
                MigrationCompleted = info.MigrationCompleted,
                Name = info.Name,
                StepsCompleted = info.StepsCompleted,
                TotalMigrationTimeInMs = info.TotalMigrationTimeInMs
            };
        }
        #endregion
    }
}