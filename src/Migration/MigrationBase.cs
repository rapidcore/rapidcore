using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RapidCore.Mongo.Migration.Internal;

namespace RapidCore.Mongo.Migration
{
    public abstract class MigrationBase : IMigration
    {
        protected MigrationBase()
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Apply this upgrade migration to the underlying database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task UpgradeAsync(MigrationContext context)
        {
            this.Context = context;

            var builder = new MigrationBuilder();
            ConfigureUpgrade(builder);
            (var stepsToApply, var doc) = await GetPendingStepsAsync(builder, context.ConnectionProvider);

            doc.Name = Name;
            var connection = context.ConnectionProvider.Default();

            foreach (var step in stepsToApply)
            {
                // Retrieve the migration action and apply it
                var actionForMigrationStep = builder.GetActionForMigrationStep(step);
                await actionForMigrationStep();

                // Add the completion to the database to ensure that we can reenter if later steps fail
                doc.StepsCompleted.Add(step);
                await connection.UpsertAsync(MigrationDocument.CollectionName, doc, document => document.Name == doc.Name);
            }
        }

        /// <summary>
        /// When implemented in a downstream migration it will configure the steps required for the upgrade migration
        /// </summary>
        /// <param name="builder"></param>

        public abstract void ConfigureUpgrade(IMigrationBuilder builder);

        /// <summary>
        /// Apply a migration downgrade towards the underlying database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task DowngradeAsync(MigrationContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When implemented in a downstream migration it will configure the steps required for the DOWN migration
        /// </summary>
        /// <param name="builder"></param>

        public abstract void ConfigureDowngrade(IMigrationBuilder builder);

        #region Helper methods
        /// <summary>
        /// Calculate which steps are currently pending from this migration
        /// </summary>
        /// <remarks>
        /// The underlying database will be queried to return the stored migration document on which the already executed steps are stored.
        /// 
        /// In cases where this is the first time the migration is executed, no document will be stored and thus all steps should be applied
        /// </remarks>
        /// <param name="builder">The migration builder instance</param>
        /// <param name="connectionProvider">The current connection provder</param>
        /// <returns>A list of the pending steps as well as an instantiataed <see cref="MigrationDocument"/></returns>
        public async Task<(IList<string>, MigrationDocument)> GetPendingStepsAsync(MigrationBuilder builder, IConnectionProvider connectionProvider)
        {
            var allStepNames = builder.GetAllStepNames();
            var connection = connectionProvider.Default();

            var migrationDocument = await connection
                .FirstOrDefaultAsync<MigrationDocument>(
                    MigrationDocument.CollectionName,
                    document => document.Name == this.Name);

            if (migrationDocument != null)
            {
                var filteredSteps = allStepNames.Except(migrationDocument.StepsCompleted).ToList();
                return (filteredSteps, migrationDocument);
            }
            return (allStepNames, new MigrationDocument());
        }
        #endregion Helper methods

        #region props

        /// <summary>
        /// Get the name of this migration
        /// </summary>
        public string Name => GetType().Name;

        /// <summary>
        /// The migration context injected when initiating the up or down phase of the migration
        /// </summary>
        protected MigrationContext Context { get; set; }
        #endregion
    }
}
