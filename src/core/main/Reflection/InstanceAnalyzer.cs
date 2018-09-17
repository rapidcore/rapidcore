using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RapidCore.Reflection
{
    /// <summary>
    /// This analyzes an instance of something, meaning that
    /// it uses reflection to find all members of the instance
    /// and tells you about it through <see cref="IInstanceListener"/>.
    ///
    /// It does not do anything with what it finds, that is up to you, but
    /// it will...
    ///  - follow complex values (i.e. recurse into other instances held by the members)
    ///  - notify you if it hits a given maximum depth during recursion
    ///  - analyze elements in enumerables, including recursing complex values
    /// </summary>
    public class InstanceAnalyzer
    {
        /// <summary>
        /// Run the analyzer on the given instance
        /// </summary>
        /// <param name="instance">The object instance you want to analyze</param>
        /// <param name="maxDepth">The max depth you find appropriate</param>
        /// <param name="listener">The listener that will be notified when we find something</param>
        public virtual void AnalyzeInstance(object instance, int maxDepth, IInstanceListener listener)
        {
            var context = new InstanceAnalyzerContext
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
        
        private void Worker(object instance, IInstanceListener listener, InstanceAnalyzerContext context)
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
                FieldAndPropertyHandler(listener, propertyInfo, context, instance);
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
        private void CallListener(IInstanceListener listener, InstanceAnalyzerContext context, MemberInfo memberInfo, Func<object> valueGetter)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                listener.OnField((FieldInfo) memberInfo, valueGetter, context);
            }
            else
            {
                listener.OnProperty((PropertyInfo) memberInfo, valueGetter, context);
            }
        }

        /// <summary>
        /// Field and properties are actually quite similar
        /// in this context. This method contains "generic" handling
        /// of both types, so we avoid duplication. 
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if we somehow try to iterate something that is not an IEnumerable</exception>
        private void FieldAndPropertyHandler(IInstanceListener listener, MemberInfo memberInfo, InstanceAnalyzerContext context, object instance)
        {
            Func<object> valueGetter = () => memberInfo.GetValue(instance);
            
            CallListener(listener, context, memberInfo, valueGetter);
            
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
                            CallListener(listener, context, memberInfo, () => entry.Value);

                            if (ShouldRecurse(entry.Value.GetType()))
                            {
                                if (context.CanGoDeeper())
                                {
                                    Worker(entry.Value, listener, context);
                                }
                                else
                                {
                                    listener.OnMaxDepth(context);
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
                            context.BreadcrumbStack.Push($"{memberInfo.Name}[{index}]");
                            CallListener(listener, context, memberInfo, () => element);

                            if (ShouldRecurse(element.GetType()))
                            {
                                if (context.CanGoDeeper())
                                {
                                    Worker(element, listener, context);
                                }
                                else
                                {
                                    listener.OnMaxDepth(context);
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
                    listener.OnMaxDepth(context);
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
            return !nonRecursedTypes.ContainsKey(type) && !type.GetTypeInfo().IsEnum;
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
    }
}