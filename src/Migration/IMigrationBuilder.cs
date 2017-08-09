using System;
using System.Threading.Tasks;

namespace RapidCore.Mongo.Migration
{
    public interface IMigrationBuilder
    {
        /// <summary>
        /// Add a named step to the migration
        /// </summary>
        /// <param name="name">The name of the migration step.</param>
        /// <param name="action">The action to execute which is the actual migration code to run.</param>
        IMigrationBuilder Step(string name, Action action);

        /// <summary>
        /// Add a named step to the migration
        /// </summary>
        /// <param name="name">The name of the migration step.</param>
        /// <param name="action">The action to execute which is the actual migration code to run.</param>
        IMigrationBuilder Step(string name, Func<Task> action);
    }
}