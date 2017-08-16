using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RapidCore.Migration.Internal;

namespace RapidCore.Migration
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
        public virtual async Task UpgradeAsync(IMigrationContext context)
        {
            this.Context = context;

            var builder = new MigrationBuilder();
            ConfigureUpgrade(builder);
            
            (var stepsToApply, var info) = await GetPendingStepsAsync(builder);

            info.Name = Name; // in case this is a new MigrationInfo

            foreach (var step in stepsToApply)
            {
                // Retrieve the migration action
                var actionForMigrationStep = builder.GetActionForMigrationStep(step);
                
                // Run the action
                await actionForMigrationStep();

                // Add the completion to storage to ensure that we can re-enter if later steps fail
                info.AddCompletedStep(step);
                await UpsertMigrationInfoAsync(info);
            }
        }

        /// <summary>
        /// When implemented in a downstream migration it will configure the steps required for the upgrade migration
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureUpgrade(IMigrationBuilder builder);

        /// <summary>
        /// Apply a migration downgrade
        /// </summary>
        /// <param name="context"></param>
        public virtual Task DowngradeAsync(IMigrationContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When implemented in a downstream migration it will configure the steps required for the DOWN migration
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureDowngrade(IMigrationBuilder builder);

        #region Helper methods
        /// <summary>
        /// Calculate which steps are currently pending from this migration
        /// </summary>
        /// <remarks>
        /// The underlying storage will be queried to return the stored migration information on which the already executed steps are recorded.
        /// 
        /// In cases where this is the first time the migration is executed, no document will be stored and thus all steps should be applied
        /// </remarks>
        /// <param name="builder">The migration builder instance</param>
        /// <returns>A list of the pending steps as well as an instantiataed <see cref="MigrationInfo"/></returns>
        public virtual async Task<(IList<string>, MigrationInfo)> GetPendingStepsAsync(MigrationBuilder builder)
        {
            var allStepNames = builder.GetAllStepNames();

            var info = await GetMigrationInfoAsync();
            
            if (info != null)
            {
                var filteredSteps = allStepNames.Except(info.StepsCompleted).ToList();
                return (filteredSteps, info);
            }
            return (allStepNames, new MigrationInfo());
        }

        /// <summary>
        /// Get the <see cref="MigrationInfo"/> instance for this migration
        /// or <c>null</c> if such an instance does not exist yet.
        /// 
        /// Tip to implementors: use <see cref="Context"/> and <see cref="Name"/>.
        /// </summary>
        protected abstract Task<MigrationInfo> GetMigrationInfoAsync();

        /// <summary>
        /// Save (insert or update) the given <paramref name="info"/>
        /// </summary>
        /// <param name="info">The migration info to save</param>
        protected abstract Task UpsertMigrationInfoAsync(MigrationInfo info);
        #endregion Helper methods

        #region props
        /// <summary>
        /// Get the name of this migration
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// The migration context injected when initiating the up or down phase of the migration
        /// </summary>
        protected IMigrationContext Context { get; set; }
        #endregion
    }
}
