using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidCore.Migration
{
    /// <summary>
    /// Finds migrations
    /// </summary>
    public interface IMigrationFinder
    {
        /// <summary>
        /// Find migrations that have yet to be run
        /// </summary>
        /// <param name="context">The context of the migration</param>
        /// <returns>A list of unrun migrations ordered by how they should be run</returns>
        Task<IList<IMigration>> FindMigrationsForUpgradeAsync(IMigrationContext context);
    }
}