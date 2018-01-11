namespace RapidCore.Migration
{
    public interface IMigrationEnvironment
    {
        string Environment { get; }

        bool IsTesting();

        bool IsStaging();
        
        bool IsProduction();
    }
}