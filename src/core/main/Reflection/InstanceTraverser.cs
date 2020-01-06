using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RapidCore.Reflection
{
    /// <summary>
    /// This traverses an instance of something, meaning that
    /// it uses reflection to find all members of the instance
    /// and tells you about it through <see cref="IInstanceListener"/>.
    ///
    /// It does not do anything with what it finds, that is up to you, but
    /// it will...
    ///  - follow complex values (i.e. recurse into other instances held by the members)
    ///  - notify you if it hits a given maximum depth during recursion
    ///  - traverse elements in enumerables, including recursing complex values
    ///  - traverse dictionaries, including recursing complex values
    /// </summary>
    public class InstanceTraverser
    {
        /// <summary>
        /// Traverse the given instance
        /// </summary>
        /// <param name="instance">The object instance you want to traverse</param>
        /// <param name="maxDepth">The max depth you find appropriate</param>
        /// <param name="listener">The listener that will be notified when we find something</param>
        public virtual void TraverseInstance(object instance, int maxDepth, IInstanceListener listener)
        {
            var context = new InstanceTraversalContext
            {
                Instance = instance,
                MaxDepth = maxDepth
            };
            
            //
            // constructors
            // we are doing constructors in the non-recursive part
            // as we are not interested in constructors of field
            // and property types
            //
            var constructors = instance.GetType().GetTypeInfo().GetConstructors();
            foreach (var constructorInfo in constructors)
            {
                listener.OnConstructor(constructorInfo, context);
            }
            
            Worker(instance, listener, context);
        }
        
        private void Worker(object instance, IInstanceListener listener, InstanceTraversalContext context)
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            
            var typeInfo = instance.GetType().GetTypeInfo();

            //
            // fields
            //
            var fields = typeInfo.GetFields(bindingFlags);
            foreach (var fieldInfo in fields)
            {
                if (!IsBackingField(fieldInfo))
                {
                    FieldAndPropertyHandler(listener, fieldInfo, context, instance);
                }
            }
            
            //
            // properties
            //
            var props = typeInfo.GetProperties(bindingFlags);
            foreach (var propertyInfo in props)
            {
                if (!IsIndexProperty(propertyInfo))
                {
                    FieldAndPropertyHandler(listener, propertyInfo, context, instance);
                }
            }
            
            //
            // methods
            //
            var methods = typeInfo.GetMethods(bindingFlags);
            foreach (var methodInfo in methods)
            {
                if (
                    methodInfo.DeclaringType != typeof(System.Object) // do not "announce" methods like Object.ToString
                    && !IsBackingMethod(methodInfo) // do not "announce" auto property backing methods
                )
                {
                    listener.OnMethod(methodInfo, context);
                }
            }
        }

        /// <summary>
        /// Call OnField or OnProperty for the given member
        /// </summary>
        private static IInstanceListenerOnFieldOrPropResult CallListener(IInstanceListener listener, InstanceTraversalContext context, MemberInfo memberInfo, Func<object> valueGetter)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return listener.OnField((FieldInfo) memberInfo, valueGetter, context);
            }

            return listener.OnProperty((PropertyInfo) memberInfo, valueGetter, context);
        }

        /// <summary>
        /// Field and properties are actually quite similar
        /// in this context. This method contains "generic" handling
        /// of both types, so we avoid duplication. 
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if we somehow try to iterate something that is not an IEnumerable</exception>
        private void FieldAndPropertyHandler(IInstanceListener listener, MemberInfo memberInfo, InstanceTraversalContext context, object instance)
        {
            Func<object> valueGetter = () => memberInfo.GetValue(instance);

            if (!CallListener(listener, context, memberInfo, valueGetter).DoContinueRecursion)
            {
                return;
            }
            
            var valueType = memberInfo.GetTypeOfValue();

            if (ShouldIterate(valueType))
            {
                object value = valueGetter.Invoke();
                
                if (value != null)
                {
                    if (value is IDictionary)
                    {
                        var dictionary = value as IDictionary;

                        foreach (DictionaryEntry entry in dictionary)
                        {
                            context.BreadcrumbStack.Push($"{memberInfo.Name}[{entry.Key}]");
                            
                            if (CallListener(listener, context, memberInfo, () => entry.Value).DoContinueRecursion)
                            {
                                
                                if (ShouldRecurse(entry.Value.GetType()))
                                {
                                    if (context.CanGoDeeper())
                                    {
                                        Worker(entry.Value, listener, context);
                                    }
                                    else
                                    {
                                        listener.OnMaxDepthReached(context);
                                    }
                                }
                                
                            }
                            
                            context.BreadcrumbStack.Pop();
                        }
                    }
                    else
                    {
                        var enumerable = value as IEnumerable;

                        if (enumerable == null)
                        {
                            throw new InvalidOperationException($"Trying to iterate over {context.BreadcrumbAsString}.{memberInfo.Name}, but could not cast to IEnumerable.");
                        }

                        var index = 0;
                        foreach (var element in enumerable)
                        {
                            // If TraverseInstance is called directly on a list with capacity greater than the size
                            // Then null elements will be present
                            if (element == null)
                            {
                                continue;
                            }
                            context.BreadcrumbStack.Push($"{memberInfo.Name}[{index}]");
                            
                            if (CallListener(listener, context, memberInfo, () => element).DoContinueRecursion)
                            {
                                
                                if (ShouldRecurse(element.GetType()))
                                {
                                    if (context.CanGoDeeper())
                                    {
                                        Worker(element, listener, context);
                                    }
                                    else
                                    {
                                        listener.OnMaxDepthReached(context);
                                    }
                                }                                
                                
                            }

                            context.BreadcrumbStack.Pop();
                            index++;
                        }
                    }
                }
            }

            else if (ShouldRecurse(valueType))
            {
                //
                // recursion
                //
                if (context.CanGoDeeper())
                {
                    object value = valueGetter.Invoke();
                    
                    if (value != null)
                    {
                        context.BreadcrumbStack.Push(memberInfo.Name);
                        Worker(value, listener, context);
                        context.BreadcrumbStack.Pop();
                    }
                }
                else
                {
                    listener.OnMaxDepthReached(context);
                }
            }
        }

        /// <summary>
        /// A quick lookup of types that we do not want to recurse.
        ///
        /// The value part is not used and means nothing
        /// </summary>
        private readonly Dictionary<Type, bool> nonRecursedTypes = new Dictionary<Type, bool>
        {
            { typeof(char), true },
            { typeof(bool), true },
            { typeof(byte), true },
            { typeof(short), true },
            { typeof(int), true },
            { typeof(long), true },
            { typeof(float), true },
            { typeof(double), true },
            { typeof(decimal), true },
            { typeof(DateTime), true },
            { typeof(DateTimeOffset), true },
            { typeof(TimeSpan), true },
            { typeof(Guid), true },
            { typeof(string), true }
        };
        
        /// <summary>
        /// Is the given type a complex type, that we should
        /// dig into?
        /// </summary>
        private bool ShouldRecurse(Type type)
        {
            return !nonRecursedTypes.ContainsKey(type) // is not "blacklisted" 
                   && !type.GetTypeInfo().IsEnum  // is not an enum
                   && !type.IsNullable() // is not nullable
                   && !type.IsStream(); // is not a Stream 
        }

        /// <summary>
        /// Should we try to iterate over the given type?
        /// </summary>
        private bool ShouldIterate(Type type)
        {
            return type != typeof(string) && (type.ImplementsInterface(typeof(IEnumerable)) || type == typeof(IEnumerable));
        }

        private static readonly Regex BackingFieldNameRegex = new Regex("^\\<[a-zA-Z0-9_]+>k__BackingField$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        
        /// <summary>
        /// Is the given field a backing field - i.e.
        /// a field implemented by the compiler for an
        /// auto-property?
        /// </summary>
        private bool IsBackingField(FieldInfo field)
        {
            return BackingFieldNameRegex.IsMatch(field.Name);
        }

        private static readonly Regex BackingMethodNameRegex = new Regex("^(set_|get_)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        
        /// <summary>
        /// Is the given method a backing method - i.e.
        /// a method implemented by the compiler for an
        /// auto-property?
        /// </summary>
        private bool IsBackingMethod(MethodBase method)
        {
            return BackingMethodNameRegex.IsMatch(method.Name);
        }

        /// <summary>
        /// Is the given property an index property - i.e.
        /// a property that requires additional parameters
        /// in order to access it.
        /// </summary>
        private bool IsIndexProperty(PropertyInfo prop)
        {
            return prop.GetIndexParameters().Length > 0;
        }
    }
}