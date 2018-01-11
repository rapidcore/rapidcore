using System;
using System.Threading.Tasks;

namespace RapidCore.Migration.Internal
{
    /// <summary>
    /// Simple data class for carrying the name of a migration step and the action to invoke when applying that step
    /// </summary>
    public class MigrationStep
    {
        public MigrationStep(string name, Func<Task> action)
        {
            Name = name;
            Action = action;
        }

        public string Name { get; }
        public Func<Task> Action { get; }
    }
}