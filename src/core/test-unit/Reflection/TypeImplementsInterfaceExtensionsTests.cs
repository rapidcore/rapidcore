using Castle.Core.Logging;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class TypeImplementsInterfaceExtensionsTests
    {
        [Fact]
        public void ReturnsFalse_ifItDoesNot()
        {
            Assert.False(typeof(string).ImplementsInterface(typeof(ILoggerFactory)));
        }
        
        [Fact]
        public void ReturnsTrue_ifTypeItself_implementsTheInterface()
        {
            Assert.True(typeof(DirectImpl).ImplementsInterface(typeof(IAmSimple)));
        }
        
        [Fact]
        public void ReturnsTrue_ifType_implementsTheInterface_via_anotherInterface()
        {
            Assert.True(typeof(InterfaceInheritance).ImplementsInterface(typeof(IAmSimple)));
        }
        
        [Fact]
        public void ReturnsTrue_ifParentType_implementsTheInterface()
        {
            Assert.True(typeof(HasParent).ImplementsInterface(typeof(IAmSimple)));
        }
        
        #region POCOs
        public interface IAmSimple
        {
        }
        
        public interface IAlsoAmSimple : IAmSimple
        {
            
        }
        
        public class DirectImpl : IAmSimple
        {
        }
        
        public class InterfaceInheritance : IAlsoAmSimple
        {
        }

        public class HasParent : DirectImpl
        {
        }
        #endregion
    }
}