using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Represents something listening while <see cref="InstanceTraverser"/>
    /// analyzes an instance of something.
    /// </summary>
    public interface IInstanceListener
    {
        void OnConstructor(ConstructorInfo ctor, IReadOnlyInstanceTraversalContext context);
        void OnField(FieldInfo field, Func<object> valueGetter, IReadOnlyInstanceTraversalContext context);
        void OnProperty(PropertyInfo property, Func<object> valueGetter, IReadOnlyInstanceTraversalContext context);
        void OnMethod(MethodInfo method, IReadOnlyInstanceTraversalContext context);
        
        /// <summary>
        /// This is called when the max depth is reached
        /// </summary>
        void OnMaxDepthReached(IReadOnlyInstanceTraversalContext context);
    }
}