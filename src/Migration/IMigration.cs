using System.Threading.Tasks;

namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Defines a migration
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Code that is run when upgrading the environment
        /// </summary>
        Task UpgradeAsync(MigrationContext context);
        
        /// <summary>
        /// Code that is run when downgrading (or rolling back) the environment
        /// </summary>
        Task DowngradeAsync(MigrationContext context);

        string Name { get; }
    }
}