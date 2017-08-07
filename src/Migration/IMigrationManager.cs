using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Finds migrations :)
    /// </summary>
    public interface IMigrationManager
    {
        /// <summary>
        /// Find migrations that have yet to be run
        /// </summary>
        /// <returns>A list of unrun migrations ordered by how they should be run</returns>
        Task<IList<IMigration>> FindMigrationsForUpgradeAsync();

        /// <summary>
        /// Mark a migration as having been completed.
        /// </summary>
        /// <param name="migration">The migration</param>
        /// <param name="milliseconds">How long the run took in milliseconds</param>
        Task MarkAsCompleteAsync(IMigration migration, long milliseconds);
    }
}