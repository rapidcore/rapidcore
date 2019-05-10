namespace RapidCore.Migration
{
    public class MigrationEnvironment : IMigrationEnvironment
    {
        public MigrationEnvironment(string environmentName)
        {
            Environment = environmentName;
        }
        
        public string Environment { get; protected set; }
        
        public bool IsDevelopment()
        {
            return Environment.ToLowerInvariant().Equals("development");
        }
        
        public bool IsCi()
        {
            return Environment.ToLowerInvariant().Equals("ci");
        }
        
        public bool IsTesting()
        {
            return Environment.ToLowerInvariant().Equals("testing");
        }

        public bool IsStaging()
        {
            return Environment.ToLowerInvariant().Equals("staging");
        }

        public bool IsProduction()
        {
            return Environment.ToLowerInvariant().Equals("production");
        }
    }
}