using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Represents something listening while <see cref="InstanceAnalyzer"/>
    /// analyzes an instance of something.
    /// </summary>
    public interface IInstanceListener
    {
        void OnConstructor(ConstructorInfo ctor, IReadOnlyInstanceAnalyzerContext context);
        void OnField(FieldInfo field, Func<object> valueGetter, IReadOnlyInstanceAnalyzerContext context);
        void OnProperty(PropertyInfo property, Func<object> valueGetter, IReadOnlyInstanceAnalyzerContext context);
        void OnMethod(MethodInfo method, IReadOnlyInstanceAnalyzerContext context);
        
        /// <summary>
        /// This is called when the max depth is reached
        /// </summary>
        void OnMaxDepth(IReadOnlyInstanceAnalyzerContext context);
    }
}