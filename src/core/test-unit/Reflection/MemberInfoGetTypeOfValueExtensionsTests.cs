using System;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class MemberInfoGetTypeOfValueExtensionsTests
    {
        private int MyField = 666;
        private bool? MyNullableField = true;
        private string MyProp => "Sup Lucifer";
        private int? MyNullableProp => null;

        public MemberInfoGetTypeOfValueExtensionsTests()
        {
            // just here for testing purposes
        }
        
        [Fact]
        public void GetTypeOfValue_onFieldInfo_works()
        {
            var memberInfo = GetField("MyField");
            
            Assert.Equal(typeof(int), memberInfo.GetTypeOfValue());
        }
        
        [Fact]
        public void GetTypeOfValue_onFieldInfo_works_withNullable()
        {
            var memberInfo = GetField("MyNullableField");
            
            Assert.Equal(typeof(bool?), memberInfo.GetTypeOfValue());
        }
        
        [Fact]
        public void GetTypeOfValue_onPropertyInfo_works()
        {
            var memberInfo = GetProp("MyProp");
            
            Assert.Equal(typeof(string), memberInfo.GetTypeOfValue());
        }
        
        [Fact]
        public void GetTypeOfValue_onPropertyInfo_works_withNullable()
        {
            var memberInfo = GetProp("MyNullableProp");
            
            Assert.Equal(typeof(int?), memberInfo.GetTypeOfValue());
        }
        
        [Fact]
        public void GetTypeOfValue_onConstructorInfo_throws()
        {
            var memberInfo = GetConstructor();

            var actual = Record.Exception(() => memberInfo.GetTypeOfValue());

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal($"Do not know how to GetTypeOfValue on a MemberInfo for a {MemberTypes.Constructor}", actual.Message);
        }
        
        [Fact]
        public void GetTypeOfValue_onMethodInfo_throws()
        {
            var memberInfo = GetMethod("GetField", new []{typeof(string)});

            var actual = Record.Exception(() => memberInfo.GetTypeOfValue());

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal($"Do not know how to GetTypeOfValue on a MemberInfo for a {MemberTypes.Method}", actual.Message);
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