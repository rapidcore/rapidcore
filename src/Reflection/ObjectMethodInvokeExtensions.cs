using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidCore.Reflection
{
    /// <summary>
    /// Extension methods for invoking methods through <see cref="System.Object" />
    /// </summary>
    public static class ObjectMethodInvokeExtensions
    {
        /// <summary>
        /// Invoke a method recursively
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

        /// <summary>
        /// Invoke a generic method recursively
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="genericTypes">The generic type arguments</param>
        /// <param name="args">The arguments for the method</param>
        /// <returns></returns>
        public static object InvokeGenericMethodRecursively(this object instance, string methodName, Type[] genericTypes, params object[] args)
        {
            return instance
                .GetType()
                .GetMethodRecursively(methodName, GetTypeArray(args))
                .MakeGenericMethod(genericTypes)
                .Invoke(instance, args);
        }

        /// <summary>
        /// Invoke property getter recursively
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property</returns>
        /// <exception cref="System.MissingMethodException">Thrown if the property does not have a getter</exception>
        /// <exception cref="System.MissingMemberException">Thrown if the property does not exist</exception>
        public static object InvokeGetterRecursively(this object instance, string propertyName)
        {
            var method = instance
                .GetType()
                .GetPropertyRecursively(propertyName)
                .GetMethod;

            if (method == null)
            {
                throw new MissingMethodException($"The property {propertyName} does not have a getter");
            }

            return method.Invoke(instance, null);
        }

        /// <summary>
        /// Invoke property setter recursively
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="propertyName">The name of the property</param>
        /// <parma name="value">The value to set</param>
        /// <exception cref="System.MissingMethodException">Thrown if the property does not have a setter</exception>
        /// <exception cref="System.MissingMemberException">Thrown if the property does not exist</exception>
        public static void InvokeSetterRecursively(this object instance, string propertyName, object value)
        {
            var method = instance
                .GetType()
                .GetPropertyRecursively(propertyName)
                .SetMethod;

            if (method == null)
            {
                throw new MissingMethodException($"The property {propertyName} does not have a setter");
            }

            method.Invoke(instance, new object[] { value });
        }

        private static Type[] GetTypeArray(object[] args)
        {
            var list = new List<Type>();

            foreach (var a in args)
            {
                if (a == null)
                {
                    list.Add(null);
                }
                else
                {
                    list.Add(a.GetType());
                }
            }

            return list.ToArray();
        }
    }
}