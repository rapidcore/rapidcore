using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RapidCore.Reflection.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute(this PropertyInfo prop, Type attribute)
        {
            return prop.GetSpecificAttribute(attribute).Count > 0;
        }

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