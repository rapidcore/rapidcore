using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for working with attributes on PropertyInfo
    /// </summary>
    public static class PropertyInfoAttributesExtensions
    {
        /// <summary>
        /// Does the property have any instances of the given attribute?
        /// </summary>
        /// <param name="prop">The property</param>
        /// <param name="attribute">The attribute to check for</param>
        /// <returns><c>True</c> if the property has the attribute, <c>false</c> otherwise</returns>
        public static bool HasAttribute(this PropertyInfo prop, Type attribute)
        {
            return prop.GetSpecificAttribute(attribute).Count > 0;
        }

        /// <summary>
        /// Get instances of the given instance
        /// </summary>
        /// <param name="prop">The property</param>
        /// <param name="attribute">The attribute to look for</param>
        /// <returns>A list of attribute instances</returns>
        public static List<Attribute> GetSpecificAttribute(this PropertyInfo prop, Type attribute)
        {
            return (
                from a in prop.GetCustomAttributes()
                where a.GetType() == attribute
                select a
            ).ToList();
        }
    }
}