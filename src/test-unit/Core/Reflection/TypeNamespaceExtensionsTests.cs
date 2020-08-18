using RapidCore.Reflection;
using TheRootNamespace;
using TheRootNamespace.SubOne;
using TheRootNamespace.SubOne.Two.Three.Four.Five.Six.Seven;
using Xunit;

namespace UnitTests.Core.Reflection
{
    public class TypeNamespaceExtensionsTests
    {
        [Fact]
        public void NamespaceWithoutRoot_works_withSomethingThatHasNoNamespace()
        {
            var actual = typeof(NoNamespace).NamespaceWithoutRoot();
            
            Assert.Equal(string.Empty, actual);
        }
        
        [Fact]
        public void NamespaceWithoutRoot_works_withSomethingInTheRoot()
        {
            var actual = typeof(InDaRoot).NamespaceWithoutRoot();
            
            Assert.Equal(string.Empty, actual);
        }
        
        [Fact]
        public void NamespaceWithoutRoot_works_withSomethingInASubNamespace()
        {
            var actual = typeof(InSubOne).NamespaceWithoutRoot();
            
            Assert.Equal("SubOne", actual);
        }
        
        [Fact]
        public void NamespaceWithoutRoot_works_withSomethingInADeeeeepSubNamespace()
        {
            var actual = typeof(GoDeep).NamespaceWithoutRoot();
            
            Assert.Equal("SubOne.Two.Three.Four.Five.Six.Seven", actual);
        }
    }
}


#region Victims

public class NoNamespace { }

namespace TheRootNamespace
{
    public class InDaRoot { }
}

namespace TheRootNamespace.SubOne
{
    public class InSubOne { }
}

namespace TheRootNamespace.SubOne.Two.Three.Four.Five.Six.Seven
{
    public class GoDeep { }
}
#endregion