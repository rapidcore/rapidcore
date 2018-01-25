using RapidCore.Migration;

namespace RapidCore.Redis.Migration
{
    public class RedisMigrationContextFactory : IMigrationContextFactory
    {
        public IMigrationContext GetContext()
        {
            return new RedisMigrationContext();
        }
    }
}