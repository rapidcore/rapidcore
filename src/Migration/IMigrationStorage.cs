using System.Threading.Tasks;

namespace RapidCore.Migration
{
    /// <summary>
    /// Storage "driver" for the migration stack
    /// </summary>
    public interface IMigrationStorage
    {
        /// <summary>
        /// Mark a migration as having been completed.
        /// </summary>
        /// <param name="context">The context of the migration</param>
        /// <param name="migration">The migration</param>
        /// <param name="milliseconds">How long the run took in milliseconds</param>
        Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds);

        /// <summary>
        /// Get the <see cref="MigrationInfo"/> instance for this migration
        /// or <c>null</c> if such an instance does not exist yet.
        /// </summary>
        /// <param name="context">The context of the migration</param>
        /// <param name="migrationName">The name of the migration</param>
        Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName);

        /// <summary>
        /// Save (insert or update) the given <paramref name="info"/>
        /// </summary>
        /// <param name="context">The context of the migration</param>
        /// <param name="info">The migration info to save</param>
        Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info);

        /// <summary>
        /// Check whether or not a migration has been fully completed (i.e. all steps have been run successfully).
        /// </summary>
        /// <param name="context">The context of the migration</param>
        /// <param name="migrationName">The name of the migration</param>
        /// <returns><c>True</c> if the migration has been fully completed, <c>false</c> otherwise.</returns>
        Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName);
    }
}