namespace RapidCore.Mongo.Migration
{
    public interface IMigrationEnvironment
    {
        string Environment { get; }

        bool IsTesting();

        bool IsStaging();
        
        bool IsProduction();
    }
}