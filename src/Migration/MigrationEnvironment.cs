using ServiceStack;

namespace RapidCore.Mongo.Migration
{
    public class MigrationEnvironment : IMigrationEnvironment
    {
        public MigrationEnvironment(string environmentName)
        {
            Environment = environmentName;
        }
        
        public string Environment { get; protected set; }
        
        public bool IsTesting()
        {
            return Environment.EqualsIgnoreCase("testing");
        }

        public bool IsStaging()
        {
            return Environment.EqualsIgnoreCase("staging");
        }

        public bool IsProduction()
        {
            return Environment.EqualsIgnoreCase("production");
        }
    }
}