using System;
using System.Linq;
using System.Reflection;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for getting a method from a <see cref="System.Type" />
    /// </summary>
    public static class TypeGetMethodRecursivelyExtensions
    {
        /// <summary>
        /// Get a method recursively
        /// </summary>
        /// <param name="type">The type to work on</param>
        /// <param name="methodName">The name of the method to get</param>
        /// <param name="argTypes">The type of the arguments you would use</param>
        /// <returns>A <see cref="System.Reflection.MethodInfo" /></returns>
        /// <exception cref="System.MissingMethodException">Thrown if the method could not be found</exception>
        public static MethodInfo GetMethodRecursively(this Type type, string methodName, params Type[] argTypes)
        {
            var typeInfo = type.GetTypeInfo();
            // var method = typeInfo.GetMethod(methodName, argTypes);

            MethodInfo method = null;
            typeInfo
                .GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Count() == argTypes.Length)
                .Select(m => m)
                .ToList()
                .ForEach(m =>
                {
                    // check if the parameter types match
                    // if argsTypes[n] is null, then the corresponding parameter must be a nullable type
                });

            if (method == null && typeInfo.BaseType != null)
            {
                method = typeInfo.BaseType.GetMethodRecursively(methodName, argTypes);
            }

            if (method == null)
            {
                throw new MissingMethodException($"Could not find method {GenerateMethodParamsSignature(methodName, argTypes)}");
            }

            return method;
        }

        private static string GenerateMethodParamsSignature(string methodName, Type[] argTypes)
        {
            var typeSignature = string.Join(", ", argTypes.Select(t => t.Name));
            return $"{methodName}({typeSignature})";
        }
    }
}