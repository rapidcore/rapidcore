namespace RapidCore.Migration
{
    public interface IMigrationEnvironment
    {
        string Environment { get; }

        bool IsDevelopment();

        bool IsCi();
        
        bool IsTesting();

        bool IsStaging();
        
        bool IsProduction();
    }
}