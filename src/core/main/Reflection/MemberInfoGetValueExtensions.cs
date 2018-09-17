using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    public static class MemberInfoGetValueExtensions
    {
        /// <summary>
        /// Get the value of the member, assuming the member actually
        /// has a value (i.e. member should be a field or property).
        /// </summary>
        /// <param name="memberInfo">The MemberInfo of the field or property</param>
        /// <param name="instance">The instance on which to invoke the member</param>
        /// <returns>The value of the given member on the given instance</returns>
        /// <exception cref="NotSupportedException">Thrown if this is called on a member type that is not supported</exception>
        public static object GetValue(this MemberInfo memberInfo, object instance)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo) memberInfo).GetValue(instance);
            }
            
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo) memberInfo).GetValue(instance);
            }
            
            throw new NotSupportedException($"Do not know how to {nameof(GetValue)} on a MemberInfo for a {memberInfo.MemberType}");
        }
    }
}