using System;

namespace RapidCore.Reflection
{
    public static class TypeNamespaceExtensions
    {
        /// <summary>
        /// Get the root namespace from a Type - i.e. the first part.
        ///
        /// E.g. "Something.Kewl" => "Kewl"
        /// </summary>
        public static string NamespaceWithoutRoot(this Type type)
        {
            var ns = type.Namespace;

            if (string.IsNullOrEmpty(ns))
            {
                return string.Empty;
            }

            var firstDotIndex = ns.IndexOf('.');

            if (firstDotIndex > 0)
            {
                return ns.Substring(firstDotIndex+1);
            }
        
            return string.Empty;
        }
    }
}