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
        /// <exception cref="System.Reflection.AmbiguousMatchException">Thrown if two or more methods match the method definition</exception>
        public static MethodInfo GetMethodRecursively(this Type type, string methodName, params Type[] argTypes)
        {
            var typeInfo = type.GetTypeInfo();
            MethodInfo method = null;
            
            // Get all the methods that satisfy the method declaration
            var methods = typeInfo
                .GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == argTypes.Length)
                .ToList()
                .FindAll(m =>
                {
                    // Make sure that each parameter satisfies the method definition
                    for (var parameterIndex = 0; parameterIndex < argTypes.Length; parameterIndex++)
                    {
                        if (argTypes[parameterIndex] == null)
                        {
                            var parameter = m.GetParameters()[parameterIndex].ParameterType;
                            if (!parameter.CanBeSetToNull())
                            {
                                // If the argument given is null but the parameter is not nullable
                                return false;
                            }
                        }
                        else
                        {
                            var parameter = m.GetParameters()[parameterIndex].ParameterType;
                            if (parameter != argTypes[parameterIndex] &&
                                parameter.GetTypeOrUnderlyingNullableType() != argTypes[parameterIndex])
                            {
                                // If the argument and parameter does not match
                                return false;
                            }
                        }
                    }
                    return true;
                });

            if (methods.Count > 1)
            {
                throw new AmbiguousMatchException($"Two or more methods match the method definition {GenerateMethodParamsSignature(methodName, argTypes)}");
            }

            method = methods.FirstOrDefault();
            
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
            var typeSignature = string.Join(", ", argTypes.Select(t => t?.Name ?? "null"));
            return $"{methodName}({typeSignature})";
        }
    }
}