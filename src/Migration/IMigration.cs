using System.Threading.Tasks;

namespace RapidCore.Migration
{
    /// <summary>
    /// Defines a migration
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Code that is run when upgrading the environment
        /// </summary>
        Task UpgradeAsync(IMigrationContext context);
        
        /// <summary>
        /// Code that is run when downgrading (or rolling back) the environment
        /// </summary>
        Task DowngradeAsync(IMigrationContext context);

        string Name { get; }
    }
}