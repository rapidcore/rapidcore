using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for working with nullable types
    /// </summary>
    public static class TypeNullableExtensions
    {
        /// <summary>
        /// Is this type nullable?
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns><c>True</c> for something like <c>int?</c> and <c>false</c> for something like <c>string</c></returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Can objects of this type be set to null
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns><c>True</c> for something like <c>string</c> and <c>false</c> for something like <c>int</c></returns>
        public static bool CanBeSetToNull(this Type type)
        {
            return !(type.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(type) == null);
        }
        
        /// <summary>
        /// Get the underlying nullable type.. e.g. "int?" => "int"
        /// </summary>
        /// <param name="type">The type to work on</param>
        /// <returns>The type that is being wrapped. E.g. calling this on <c>int?</c> returns <c>int</c></returns>
        public static Type GetUnderlyingNullableType(this Type type)
        {
            return type.GenericTypeArguments[0];
        }

        /// <summary>
        /// Get the type while "looking through" nullables. In other words:
        /// if <paramref name="type"/> is a nullable, then the underlying type
        /// is returned (e.g. "int?" => "int"). Otherwise the type itself is returned.
        /// </summary>
        /// <param name="type">The type to work on</param>
        /// <returns>The underlying nullable type if <paramref name="type"/> is nullable. Otherwise
        /// <paramref name="type"/> is returned as-is.</returns>
        public static Type GetTypeOrUnderlyingNullableType(this Type type)
        {
            if (type.IsNullable())
            {
                return type.GetUnderlyingNullableType();
            }

            return type;
        }
    }
}