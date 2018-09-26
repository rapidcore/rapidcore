using System;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class MemberInfoGetValueExtensionsTests
    {
        private int MyField = 666;
        private string MyProp => "Sup Lucifer";

        public MemberInfoGetValueExtensionsTests()
        {
            // just here for testing purposes
        }
        
        [Fact]
        public void GetValue_onFieldInfo_works()
        {
            var memberInfo = GetField("MyField");
            
            Assert.Equal(666, memberInfo.GetValue(this));
        }
        
        [Fact]
        public void GetValue_onPropertyInfo_works()
        {
            var memberInfo = GetProp("MyProp");
            
            Assert.Equal("Sup Lucifer", memberInfo.GetValue(this));
        }
        
        [Fact]
        public void GetValue_onConstructorInfo_throws()
        {
            var memberInfo = GetConstructor();

            var actual = Record.Exception(() => memberInfo.GetValue(this));

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal($"Do not know how to GetValue on a MemberInfo for a {MemberTypes.Constructor}", actual.Message);
        }
        
        [Fact]
        public void GetValue_onMethodInfo_throws()
        {
            var memberInfo = GetMethod("GetField", new []{typeof(string)});

            var actual = Record.Exception(() => memberInfo.GetValue(this));

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal($"Do not know how to GetValue on a MemberInfo for a {MemberTypes.Method}", actual.Message);
        }
        
        
        
        private MemberInfo GetField(string name)
        {
            return GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
        
        private MemberInfo GetProp(string name)
        {
            return GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
        
        private MemberInfo GetConstructor()
        {
            return GetType().GetConstructor(new Type[0]);
        }
        
        private MemberInfo GetMethod(string name, Type[] argTypes)
        {
            return GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(x => x.Name.Equals(name)).Where(x => x.GetParameters().Length == argTypes.Length)
                .Select(x => x)
                .FirstOrDefault();
        }
    }
}