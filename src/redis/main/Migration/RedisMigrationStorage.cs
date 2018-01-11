using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RapidCore.Migration;
using StackExchange.Redis;

namespace RapidCore.Redis.Migration
{
    public class RedisMigrationStorage : IMigrationStorage
    {
        public static string KeyPrefix = "__rapidcore:migrations:";
        
        private IDatabase GetDatabase(IMigrationContext context)
        {
            return context.Container.Resolve<IConnectionMultiplexer>().GetDatabase();
        }

        private string GetKey(string migrationName)
        {
            return $"{KeyPrefix}{migrationName}";
        }
        
        public async Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds)
        {
            var info = await GetMigrationInfoAsync(context, migration.Name);

            if (info == default(MigrationInfo))
            {
                info = new MigrationInfo
                {
                    Name = migration.Name
                };
            }

            info.CompletedAtUtc = DateTime.UtcNow;
            info.MigrationCompleted = true;
            info.TotalMigrationTimeInMs = milliseconds;

            await UpsertMigrationInfoAsync(context, info);
        }

        public async Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName)
        {
            var key = GetKey(migrationName);
            var value = await GetDatabase(context).StringGetAsync(key);

            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<MigrationInfo>(value.ToString());
            }

            return default(MigrationInfo);
        }

        public async Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info)
        {
            var json = JsonConvert.SerializeObject(info);
            var key = GetKey(info.Name);
            
            await GetDatabase(context).StringSetAsync(key, json);
        }

        public async Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName)
        {
            var info = await GetMigrationInfoAsync(context, migrationName);

            return (info != default(MigrationInfo) && info.MigrationCompleted);
        }
    }
}
