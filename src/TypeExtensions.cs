using System;
using System.Reflection;

namespace RapidCore.Reflection
{
    public static class TypeExtensions
    {
        public static void ListMethodsRecursively(this Type type)
        {
            type.GetTypeInfo().ListMethodsRecursively();
        }
    }
}