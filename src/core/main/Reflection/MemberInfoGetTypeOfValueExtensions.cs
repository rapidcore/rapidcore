using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    public static class MemberInfoGetTypeOfValueExtensions
    {
        /// <summary>
        /// Get the type of the value contained in a member - assuming
        /// that the member actually contains something (i.e. fields and properties).
        ///
        /// See <see cref="FieldInfo.FieldType"/> and <see cref="PropertyInfo.PropertyType"/>
        /// </summary>
        /// <param name="memberInfo">The MemberInfo</param>
        /// <returns>The type of the contained value</returns>
        /// <exception cref="NotSupportedException">Thrown if this is called on a member type that is not supported</exception>
        public static Type GetTypeOfValue(this MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo) memberInfo).FieldType;
            }
            
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo) memberInfo).PropertyType;
            }
            
            throw new NotSupportedException($"Do not know how to {nameof(GetTypeOfValue)} on a MemberInfo for a {memberInfo.MemberType}");
        }
    }
}