using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RapidCore.Migration
{
    /// <summary>
    /// Implementation providing "search for migrations" using reflection. It
    /// looks for concrete classes in the provided assemblies, that implement <see cref="IMigration"/>.
    /// </summary>
    public class ReflectionMigrationFinder : IMigrationFinder
    {
        /// <summary>
        /// The assemblies provided at construction
        /// </summary>
        protected readonly IList<Assembly> assemblies;

        public ReflectionMigrationFinder(IList<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public ReflectionMigrationFinder(Assembly assembly) : this(new List<Assembly> {assembly})
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
                    var instance = GetNewMigrationInstance(type, context);

                    if (!(await context.Storage.HasMigrationBeenFullyCompletedAsync(context, instance.Name)))
                    {
                        migrations.Add(instance.Name, instance);
                    }
                }
            }

            return migrations.Values;
        }

        /// <summary>
        /// Get a new instance of an <see cref="IMigration"/>.
        /// 
        /// First, this method will attempt to get an instance
        /// from the container defined on the context. If that fails,
        /// it will create one using <see cref="Activator"/>.
        /// </summary>
        /// <param name="type">The type of the migration</param>
        /// <param name="context">The migration context</param>
        protected virtual IMigration GetNewMigrationInstance(Type type, IMigrationContext context)
        {
            var x = context.Container.Resolve(type);

            if (x != null)
            {
                return (IMigration)x;
            }

            return (IMigration)Activator.CreateInstance(type);
        }
    }
}