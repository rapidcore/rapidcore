using System;
using System.Collections.Generic;

namespace RapidCore.Mongo.Migration.Internal
{
    public interface IMigrationBuilder
    {
        /// <summary>
        /// Add a named step to the migration
        /// </summary>
        /// <param name="name">The name of the migration test.</param>
        /// <param name="action">The action to execute which is the actual migration code to run.</param>
        /// <returns></returns>
        IMigrationBuilder WithStep(string name, Action action);

        /// <summary>
        /// Get the corresponding action for a step with the given name
        /// </summary>
        /// <param name="stepName"></param>
        /// <returns></returns>
        Action GetActionForMigrationStep(string stepName);

        /// <summary>
        /// Returns a list of all the steps that have been added to the migration
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllStepNames();
    }
}
