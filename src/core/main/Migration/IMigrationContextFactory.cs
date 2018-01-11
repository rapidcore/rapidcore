namespace RapidCore.Migration
{
    public interface IMigrationContextFactory
    {
        IMigrationContext GetContext();
    }
}