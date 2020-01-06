using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.Diffing.Internal
{
    /// <summary>
    /// This class is used for finding changes between two changes
    /// </summary>
    /// <remarks>
    /// This class MUST NOT be registered as a singleton
    /// </remarks>
    public class StateChangeFinderWorker : IInstanceListener
    {
        private readonly InstanceTraverser traverser;

        private BeingTraversed beingTraversed = BeingTraversed.Nothing;
        private Dictionary<string, StateChange> values;
        private Func<FieldInfo, IReadOnlyInstanceTraversalContext, bool> fieldIgnoreFn;
        private Func<PropertyInfo, IReadOnlyInstanceTraversalContext, bool> propertyIgnoreFn;

        public StateChangeFinderWorker(InstanceTraverser traverser)
        {
            this.traverser = traverser;
            this.fieldIgnoreFn = (field, context) => false;
            this.propertyIgnoreFn = (prop, context) => false;
        }

        public virtual void FindDifferences
        (
            object oldState,
            object newState,
            StateChanges stateChanges,
            int maxDepth,
            Func<FieldInfo, IReadOnlyInstanceTraversalContext, bool> fieldIgnoreFunc = null,
            Func<PropertyInfo, IReadOnlyInstanceTraversalContext, bool> propertyIgnoreFunc = null
        )
        {
            if (fieldIgnoreFunc == null)
            {
                fieldIgnoreFunc = (field, context) => false;
            }

            if (propertyIgnoreFunc == null)
            {
                propertyIgnoreFunc = (prop, context) => false;
            }

            fieldIgnoreFn = fieldIgnoreFunc;
            propertyIgnoreFn = propertyIgnoreFunc;
            
            values = new Dictionary<string, StateChange>();

            if (oldState != null)
            {
                beingTraversed = BeingTraversed.Old;
                traverser.TraverseInstance(oldState, maxDepth, this);
            }

            if (newState != null)
            {
                beingTraversed = BeingTraversed.New;
                traverser.TraverseInstance(newState, maxDepth, this);
            }

            beingTraversed = BeingTraversed.Nothing;
            
            foreach (var kvp in values)
            {
                var change = kvp.Value;

                if (change.OldValue == null && change.NewValue != null)
                {
                    stateChanges.Changes.Add(change);
                }
                
                else if (change.OldValue != null && !change.OldValue.Equals(change.NewValue))
                {
                    stateChanges.Changes.Add(change);
                }
            }
        }

        private string GetKey(IReadOnlyInstanceTraversalContext context, MemberInfo member)
        {
            var key = context.BreadcrumbAsString;

            if (string.IsNullOrWhiteSpace(key))
            {
                return member.Name;
                
            }
            
            if (context.Breadcrumb.Last().StartsWith($"{member.Name}["))
            {
                return key;
            }

            return $"{key}.{member.Name}";
        }

        /// <summary>
        /// Set change value
        /// </summary>
        /// <param name="member">The member representing the field/prop containing the value (i.e. might point to a list
        /// whereas the value is a member of that list)</param>
        /// <param name="value">The value of the field/property _OR_ an element in a list _OR_ a value in a dictionary</param>
        /// <param name="context">Context</param>
        private void SetValue(MemberInfo member, object value, IReadOnlyInstanceTraversalContext context)
        {
            // we are checking for the type of the
            // value directly on the value instead
            // through 
            if (value != null && value.GetType().GetTypeInfo().IsClass && !(value is string))
            {
                // we do not want "changes" for objects, as the
                // coming changes for "object.field|property" are
                // much more interesting
                return;
            }

            var key = GetKey(context, member);

            if (!values.ContainsKey(key))
            {
                values.Add(
                    key, 
                    new StateChange
                    {
                        MemberInfo = member,
                        Breadcrumb = key
                    });
            }
            
            switch (beingTraversed)
            {
                    case BeingTraversed.Old:
                        values[key].OldValue = value;
                        break;
                    
                    case BeingTraversed.New:
                        values[key].NewValue = value;
                        break;
                    
                    case BeingTraversed.Nothing:
                    default:
                        throw new InvalidOperationException($"{nameof(beingTraversed)} must be set before you can {nameof(SetValue)}");
            }
        }
        
        public void OnConstructor(ConstructorInfo ctor, IReadOnlyInstanceTraversalContext context)
        {
            // we do not care
        }

        public IInstanceListenerOnFieldOrPropResult OnField(FieldInfo field, Func<object> valueGetter, IReadOnlyInstanceTraversalContext context)
        {
            if (fieldIgnoreFn.Invoke(field, context))
            {
                return new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = false };
            }
            
            SetValue(field, valueGetter.Invoke(), context);
            
            return new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true };
        }

        public IInstanceListenerOnFieldOrPropResult OnProperty(PropertyInfo property, Func<object> valueGetter, IReadOnlyInstanceTraversalContext context)
        {
            if (propertyIgnoreFn.Invoke(property, context))
            {
                return new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = false };
            }
            
            SetValue(property, valueGetter.Invoke(), context);
            
            return new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true };
        }

        public void OnMethod(MethodInfo method, IReadOnlyInstanceTraversalContext context)
        {
            // we do not care
        }

        public void OnMaxDepthReached(IReadOnlyInstanceTraversalContext context)
        {
            // by not blowing up, we make it possible
            // to continue working with the already
            // registered data, which can be useful
        }
    }
}