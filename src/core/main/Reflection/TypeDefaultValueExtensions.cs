using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    public static class TypeDefaultValueExtensions
    {
        /// <summary>
        /// Get the default value for the given type - i.e.
        /// works kind of like <c>default(char)</c> except
        /// you can do it at runtime.
        /// </summary>
        /// <param name="type">The type you want the default value for</param>
        /// <returns>The result of <c>default(x)</c>, but at runtime</returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}