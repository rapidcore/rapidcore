using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using RapidCore.Reflection;

namespace RapidCore.UnitTests.Reflection.InstanceTraverserTests
{
    public abstract class InstanceTraverserTestBase
    {
        protected readonly InstanceTraverser Traverser;
        protected readonly IInstanceListener listener;

        protected InstanceTraverserTestBase()
        {
            listener = A.Fake<IInstanceListener>();
            
            Traverser = new InstanceTraverser();
        }
        
        protected virtual ConstructorInfo GetConstructor(Type type, Type[] argTypes)
        {
            return type.GetConstructor(argTypes);
        }

        protected virtual FieldInfo GetField(Type type, string name)
        {
            return type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
        
        protected virtual PropertyInfo GetProp(Type type, string name)
        {
            return type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
        
        protected virtual MethodInfo GetMethod(Type type, string name, Type[] argTypes)
        {
            return type
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(x => x.Name.Equals(name)).Where(x => x.GetParameters().Length == argTypes.Length)
                .Select(x => x)
                .FirstOrDefault();
        }
    }
}