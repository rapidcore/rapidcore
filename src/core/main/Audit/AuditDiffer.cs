using System;
using System.Reflection;
using RapidCore.DependencyInjection;
using RapidCore.Diffing;
using RapidCore.Reflection;

namespace RapidCore.Audit
{
    /// <summary>
    /// Generates a diff between 2 state objects
    /// that is suitable for logging - i.e. does not
    /// contain sensitive data.
    /// </summary>
    public class AuditDiffer
    {
        private readonly IRapidContainerAdapter container;
        private readonly StateChangeFinder stateChangeFinder;

        public AuditDiffer(IRapidContainerAdapter container)
        {
            this.container = container;
            stateChangeFinder = new StateChangeFinder();
        }
        
        /// <summary>
        /// Generate a diff of the given objects, suitable
        /// for logging.
        ///
        /// You can control which members are included and how
        /// by marking your state objects with <see cref="AuditAttribute"/>.
        /// </summary>
        /// <param name="oldState">The old state - null is allowed</param>
        /// <param name="newState">The new state - null is allowed</param>
        /// <returns>A set of changes</returns>
        /// <exception cref="ArgumentException">Thrown if a value masker has been supplied, that does not implement the right interface</exception>
        public virtual StateChanges GetAuditReadyDiff(object oldState, object newState)
        {
            var auditChanges = new StateChanges();
            
            var changes = stateChangeFinder.GetChanges(oldState, newState);
            
            foreach (var change in changes.Changes)
            {
                if (change.MemberInfo is FieldInfo fieldInfo && !fieldInfo.IsPublic)
                {
                    // we do not want private/protected fields
                    continue;
                }

                if (change.MemberInfo is PropertyInfo propInfo && propInfo.GetMethod != null && !propInfo.GetMethod.IsPublic)
                {
                    // we do not want private/protected properties
                    continue;
                }

                if (change.MemberInfo.HasAttribute(typeof(AuditAttribute)))
                {
                    var attr = change.MemberInfo.GetCustomAttribute<AuditAttribute>();

                    if (!attr.Include)
                    {
                        continue;
                    }

                    if (attr.DoMaskValue)
                    {
                        if (!attr.ValueMasker.ImplementsInterface(typeof(IAuditValueMasker)))
                        {
                            throw new ArgumentException($"A {nameof(AuditAttribute.ValueMasker)} must implement {typeof(IAuditValueMasker).Name}, which {attr.ValueMasker.Name} does not.");
                        }
                        
                        var masker = (IAuditValueMasker)container.Resolve(attr.ValueMasker);
                        change.OldValue = masker.MaskValue(change.OldValue);
                        change.NewValue = masker.MaskValue(change.NewValue);
                    }
                }
                
                auditChanges.Changes.Add(change);
            }
            
            return auditChanges;
        }
    }
}