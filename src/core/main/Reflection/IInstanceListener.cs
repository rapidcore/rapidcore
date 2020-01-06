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
        /// <summary>
        /// This is called when a constructor is found
        /// </summary>
        /// <param name="ctor">The constructor info of the found constructor</param>
        /// <param name="context">The context of the instance traversal</param>
        void OnConstructor(ConstructorInfo ctor, IReadOnlyInstanceTraversalContext context);
        
        /// <summary>
        /// This is called when a field is found
        /// </summary>
        /// <param name="field">The field info of the found field</param>
        /// <param name="valueGetter">A function for getting the value</param>
        /// <param name="context">The context of the instance traversal</param>
        IInstanceListenerOnFieldOrPropResult OnField(FieldInfo field, Func<object> valueGetter, IReadOnlyInstanceTraversalContext context);
        
        /// <summary>
        /// This is called when a property is found
        /// </summary>
        /// <param name="property">The property info of the found property</param>
        /// <param name="valueGetter">A function for getting the value</param>
        /// <param name="context">The context of the instance traversal</param>
        IInstanceListenerOnFieldOrPropResult OnProperty(PropertyInfo property, Func<object> valueGetter, IReadOnlyInstanceTraversalContext context);

        /// <summary>
        /// This is called when a method is found
        /// </summary>
        /// <param name="method">The method info of the found method</param>
        /// <param name="context">The context of the instance traversal</param>
        void OnMethod(MethodInfo method, IReadOnlyInstanceTraversalContext context);
        
        /// <summary>
        /// This is called when the max depth is reached
        /// </summary>
        void OnMaxDepthReached(IReadOnlyInstanceTraversalContext context);
    }
}