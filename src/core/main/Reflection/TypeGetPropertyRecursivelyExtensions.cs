using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for getting properties recursively from <see cref="System.Type" />
    /// </summary>
    public static class TypeGetPropertyRecursivelyExtensions
    {
        /// <summary>
        /// Get a property recursively
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The <see cref="System.Reflection.PropertyInfo" /></returns>
        /// <exception cref="System.MissingMemberException">Thrown if the property does not exist</exception>
        public static PropertyInfo GetPropertyRecursively(this Type type, string propertyName)
        {
            var typeInfo = type.GetTypeInfo();

            var property = typeInfo.GetDeclaredProperty(propertyName);

            if (property == null && typeInfo.BaseType != null)
            {
                return typeInfo.BaseType.GetPropertyRecursively(propertyName);
            }

            if (property == null)
            {
                throw new MissingMemberException($"Could not find a property called {propertyName}");
            }

            return property;
        }
    }
}