using System;
using System.IO;
using System.Reflection;

namespace RapidCore.Reflection
{
    public static class TypeIsStreamExtensions
    {
        /// <summary>
        /// Check whether the given type is some form of <see cref="Stream"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStream(this Type type)
        {
            var streamType = typeof(Stream);
            
            if (type == streamType)
            {
                return true;
            }

            var baseType = type.GetTypeInfo().BaseType;

            while (baseType != null)
            {
                if (baseType == streamType)
                {
                    return true;
                }

                baseType = baseType.GetTypeInfo().BaseType;
            }
            
            return false;
        }
    }
}