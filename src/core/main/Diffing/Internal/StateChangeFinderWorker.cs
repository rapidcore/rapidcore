using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.Diffing.Internal
{
    public class StateChangeFinderWorker : IInstanceListener
    {
        private readonly InstanceAnalyzer analyzer;

        private BeingAnalyzed beingAnalyzed = BeingAnalyzed.Nothing;
        private Dictionary<string, StateChange> values;

        public StateChangeFinderWorker(InstanceAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        public virtual void FindDifferences(object oldState, object newState, StateChanges stateChanges, int maxDepth)
        {
            values = new Dictionary<string, StateChange>();

            if (oldState != null)
            {
                beingAnalyzed = BeingAnalyzed.Old;
                analyzer.AnalyzeInstance(oldState, maxDepth, this);
            }

            if (newState != null)
            {
                beingAnalyzed = BeingAnalyzed.New;
                analyzer.AnalyzeInstance(newState, maxDepth, this);
            }

            beingAnalyzed = BeingAnalyzed.Nothing;
            
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

        private string GetKey(IReadOnlyInstanceAnalyzerContext context, MemberInfo member)
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
        private void SetValue(MemberInfo member, object value, IReadOnlyInstanceAnalyzerContext context)
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
            
            switch (beingAnalyzed)
            {
                    case BeingAnalyzed.Old:
                        values[key].OldValue = value;
                        break;
                    
                    case BeingAnalyzed.New:
                        values[key].NewValue = value;
                        break;
                    
                    case BeingAnalyzed.Nothing:
                    default:
                        throw new InvalidOperationException($"{nameof(beingAnalyzed)} must be set before you can {nameof(SetValue)}");
            }
        }
        
        public void OnConstructor(ConstructorInfo ctor, IReadOnlyInstanceAnalyzerContext context)
        {
            // we do not care
        }

        public void OnField(FieldInfo field, Func<object> valueGetter, IReadOnlyInstanceAnalyzerContext context)
        {
            SetValue(field, valueGetter.Invoke(), context);
        }

        public void OnProperty(PropertyInfo property, Func<object> valueGetter, IReadOnlyInstanceAnalyzerContext context)
        {
            SetValue(property, valueGetter.Invoke(), context);
        }

        public void OnMethod(MethodInfo method, IReadOnlyInstanceAnalyzerContext context)
        {
            // we do not care
        }

        public void OnMaxDepth(IReadOnlyInstanceAnalyzerContext context)
        {
            // by not blowing up, we make it possible
            // to continue working with the already
            // registered data, which can be useful
        }
    }
}