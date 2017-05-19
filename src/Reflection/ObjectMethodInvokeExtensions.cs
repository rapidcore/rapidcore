using System;
using System.Linq;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for invoking methods through <see cref="System.Object" />
    /// </summary>
    public static class ObjectMethodInvokeExtensions
    {
        /// <summary>
        /// Invoke a method recursively on the given instance
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns>Whatever the called method returns</returns>
        public static object InvokeMethodRecursively(this object instance, string methodName, params object[] args)
        {
            return instance
                .GetType()
                .GetMethodRecursively(methodName, GetTypeArray(args))
                .Invoke(instance, args);
        }

        private static Type[] GetTypeArray(object[] args)
        {
            return args.Select(a => a.GetType()).ToArray();
        }
    }
}