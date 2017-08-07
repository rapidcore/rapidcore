using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidCore.Mongo.Migration.Internal
{
    public class MigrationBuilder : IMigrationBuilder
    {
        public IList<MigrationStep> MigrationSteps { get; } = new List<MigrationStep>();

        /// <inheritdoc />
        /// <summary>
        /// Add a migration with the given step name and the actual migration action to run
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="action">The action to invoke for applying the migration.</param>
        /// <returns><c>this</c> to allow method chaining</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// stepName - Please provide a non null non whitespace name for your step
        /// or
        /// action - Please provided a non null action for your migration step
        /// </exception>
        /// <exception cref="T:System.ArgumentException">stepName</exception>
        public IMigrationBuilder WithStep(string stepName, Action action)
        {
            if (string.IsNullOrWhiteSpace(stepName))
            {
                throw new ArgumentNullException(
                    nameof(stepName),
                    "Please provide a non null non whitespace name for your step");
            }

            if (action == null)
            {
                throw new ArgumentNullException(
                    nameof(action),
                    "Please provided a non null action for your migration step");
            }

            var step = new MigrationStep(stepName, action);
            var existing = MigrationSteps.SingleOrDefault(q => q.Name == stepName);

            if (existing != default(MigrationStep))
            {
                throw new ArgumentException(
                    $"Cannot add a step with name {stepName} as there is already a step with that name",
                    nameof(stepName));
            }
            MigrationSteps.Add(step);

            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the action for the migration step.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <returns></returns>
        /// <exception cref="!:ArgumentException">stepName</exception>
        public Action GetActionForMigrationStep(string stepName)
        {
            var step = MigrationSteps.SingleOrDefault(q => q.Name == stepName);
            if (step == null)
            {
                throw new ArgumentException(
                    $"No step with the given name: '{stepName}' - did you add it to your migration using WithStep(...) ?",
                    nameof(stepName));
            }
            return step.Action;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllStepNames()
        {
            return MigrationSteps.Select(s => s.Name).ToList();
        }
    }
}
