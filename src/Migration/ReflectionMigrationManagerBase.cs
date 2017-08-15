using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RapidCore.Migration
{
    /// <summary>
    /// Base class providing "search for migrations" using reflection. It
    /// looks for classes in the provided assemblies, that implement <see cref="IMigration"/>.
    /// 
    /// All storage related functionality is left for the consumer to implement.
    /// </summary>
    public abstract class ReflectionMigrationManagerBase : IMigrationManager
    {
        /// <summary>
        /// The assemblies provided at construction
        /// </summary>
        protected readonly IList<Assembly> assemblies;

        protected ReflectionMigrationManagerBase(IList<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        protected ReflectionMigrationManagerBase(Assembly assembly) : this(new List<Assembly> {assembly})
        {
        }

        public virtual async Task<IList<IMigration>> FindMigrationsForUpgradeAsync(IMigrationContext context)
        {
            var migrations = new SortedList<string, IMigration>();

            foreach (var asm in assemblies)
            {
                var types = asm.ExportedTypes
                    .Where(x => !x.GetTypeInfo().IsAbstract && x.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IMigration)))
                    .Select(x => x);

                foreach (var type in types)
                {
                    var instance = GetMigrationInstance(type, context);

                    if (!(await HasMigrationBeenFullyCompletedAsync(instance.Name, context)))
                    {
                        migrations.Add(instance.Name, instance);
                    }
                }
            }

            return migrations.Values;
        }

        public abstract Task MarkAsCompleteAsync(IMigration migration, long milliseconds, IMigrationContext context);

        /// <summary>
        /// Get an instance of an <see cref="IMigration"/>.
        /// 
        /// First, this method will attempt to get an instance
        /// from the container defined on the context. If that fails,
        /// it will create one using <see cref="Activator"/>.
        /// </summary>
        /// <param name="type">The type of the migration</param>
        /// <param name="context">The migration context</param>
        protected virtual IMigration GetMigrationInstance(Type type, IMigrationContext context)
        {
            var x = context.Container.Resolve(type);

            if (x != null)
            {
                return (IMigration)x;
            }

            return (IMigration)Activator.CreateInstance(type);
        }
        
        /// <summary>
        /// Check whether or not a migration has been fully completed (i.e. all steps have been run successfully).
        /// </summary>
        /// <param name="migrationName">The name of the migration</param>
        /// <param name="context">The context of the migration</param>
        /// <returns><c>True</c> if the migration has been fully completed, <c>false</c> otherwise.</returns>
        protected abstract Task<bool> HasMigrationBeenFullyCompletedAsync(string migrationName, IMigrationContext context);
    }
}