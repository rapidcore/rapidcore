using System;

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
        IMigrationBuilder Step(string name, Action action);
    }
}