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
        Task UpgradeAsync();
        
        /// <summary>
        /// Code that is run when downgrading (or rolling back) the environment
        /// </summary>
        Task DowngradeAsync();

        string Name { get; }
    }
}