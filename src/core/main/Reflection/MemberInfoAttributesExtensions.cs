using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for working with attributes on MemberInfo
    /// </summary>
    public static class MemberInfoAttributesExtensions
    {
        /// <summary>
        /// Does the member have any instances of the given attribute?
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="attribute">The attribute to check for</param>
        /// <returns><c>True</c> if the member has the attribute, <c>false</c> otherwise</returns>
        public static bool HasAttribute(this MemberInfo member, Type attribute)
        {
            return member.GetSpecificAttribute(attribute).Count > 0;
        }

        /// <summary>
        /// Does the member have any instances of the given attribute?
        /// </summary>
        /// <param name="member">The member</param>
        /// <typeparam name="TAttribute">The attribute to check for</typeparam>
        /// <returns><c>True</c> if the member has the attribute, <c>false</c> otherwise</returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute
        {
            return member.HasAttribute(typeof(TAttribute));
        }

        /// <summary>
        /// Get instances of the given attribute
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="attribute">The attribute to look for</param>
        /// <returns>A list of attribute instances (empty if the attribute is not present)</returns>
        public static List<Attribute> GetSpecificAttribute(this MemberInfo member, Type attribute)
        {
            return (
                from a in member.GetCustomAttributes()
                where a.GetType() == attribute
                select a
            ).ToList();
        }

        /// <summary>
        /// Get instances of the given attribute
        /// </summary>
        /// <param name="member">The member</param>
        /// <typeparam name="TAttribute">The attribute to look for</typeparam>
        /// <returns>A list of attribute instances (empty if the attribute is not present)</returns>
        public static List<TAttribute> GetSpecificAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute
        {
            return member.GetSpecificAttribute(typeof(TAttribute)).Select(x => (TAttribute) x).ToList();
        }
    }
}