using System;
using System.Linq;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for checking whether a type
    /// implements a specific interface
    /// </summary>
    public static class TypeImplementsInterfaceExtensions
    {
        /// <summary>
        /// Does this type implement the given interface?
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="theInterface">The interface to check for</param>
        public static bool ImplementsInterface(this Type type, Type theInterface)
        {
            return type.GetTypeInfo().ImplementsInterface(theInterface);
        }

        /// <summary>
        /// Does this typeinfo implement the given interface?
        /// </summary>
        /// <param name="typeInfo">The type info</param>
        /// <param name="theInterface">The interface to check for</param>
        /// <returns></returns>
        public static bool ImplementsInterface(this TypeInfo typeInfo, Type theInterface)
        {
            return typeInfo
                .ImplementedInterfaces
                .Contains(theInterface);
        }
    }
}