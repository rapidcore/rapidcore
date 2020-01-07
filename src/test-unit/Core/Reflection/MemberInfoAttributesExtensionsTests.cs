using System;
using System.ComponentModel;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace UnitTests.Core.Reflection
{
    public class MemberInfoAttributesExtensionsTests
    {
        [Theory]
        [InlineData("HasObsoleteAttrib", typeof(ObsoleteAttribute), true)] // returns true when it has the attribute
        [InlineData("HasObsoleteAttrib", typeof(DisplayNameAttribute), false)] // returns false when it does not have the attribute, although it has another attribute
        [InlineData("HasNoAttrib", typeof(DisplayNameAttribute), false)] // returns false when it does not have the attribute
        public void HasAttribute(string propertyName, Type attributeType, bool expected)
        {
            Assert.Equal(expected, GetProperty(propertyName).HasAttribute(attributeType));
        }

        [Fact]
        public void HasAttribute_T_returns_true_if_attributeIsPresent()
        {
            Assert.Equal(true, GetProperty("HasObsoleteAttrib").HasAttribute<ObsoleteAttribute>());
        }
        
        [Fact]
        public void HasAttribute_T_returns_false_if_attributeIsNotPresent_evenWhenItHasOtherAttributes()
        {
            Assert.Equal(false, GetProperty("HasObsoleteAttrib").HasAttribute<DisplayNameAttribute>());
        }

        [Fact]
        public void HasAttribute_T_returns_false_ifItDoesNotHaveTheAttribute()
        {
            Assert.Equal(false, GetProperty("HasNoAttrib").HasAttribute<DisplayNameAttribute>());
        }

        [Fact]
        public void GetSpecificAttribute_returnAttributeInstance_whenItExists()
        {
            var actual = GetProperty("HasObsoleteAttrib").GetSpecificAttribute(typeof(ObsoleteAttribute));

            Assert.Equal(1, actual.Count);
            Assert.IsType(typeof(ObsoleteAttribute), actual[0]);
        }

        [Fact]
        public void GetSpecificAttribute_returnsEmptyList_whenItDoesNotHaveTheAttribute()
        {
            var actual = GetProperty("HasNoAttrib").GetSpecificAttribute(typeof(ObsoleteAttribute));

            Assert.Empty(actual);
        }

        [Fact]
        public void GetSpecificAttribute_worksOnClassLevel()
        {
            var actual = typeof(GuineaPig).GetTypeInfo().GetSpecificAttribute(typeof(DisplayNameAttribute));

            Assert.Equal(1, actual.Count);
            Assert.IsType(typeof(DisplayNameAttribute), actual[0]);
        }
        
        [Fact]
        public void GetSpecificAttribute_T_returnAttributeInstance_whenItExists()
        {
            var actual = GetProperty("HasObsoleteAttrib").GetSpecificAttribute<ObsoleteAttribute>();

            Assert.Equal(1, actual.Count);
            Assert.IsType(typeof(ObsoleteAttribute), actual[0]);
        }

        [Fact]
        public void GetSpecificAttribute_T_returnsEmptyList_whenItDoesNotHaveTheAttribute()
        {
            var actual = GetProperty("HasNoAttrib").GetSpecificAttribute<ObsoleteAttribute>();

            Assert.Empty(actual);
        }

        [Fact]
        public void GetSpecificAttribute_T_worksOnClassLevel()
        {
            var actual = typeof(GuineaPig).GetTypeInfo().GetSpecificAttribute<DisplayNameAttribute>();

            Assert.Equal(1, actual.Count);
            Assert.IsType(typeof(DisplayNameAttribute), actual[0]);
        }

        private PropertyInfo GetProperty(string propertyName)
        {
            return typeof(GuineaPig).GetTypeInfo().GetProperty(propertyName);
        }

        #region GuineaPig
        [DisplayName]
        private class GuineaPig
        {
            [Obsolete]
            public string HasObsoleteAttrib { get; set; }

            [DisplayName]
            public string HasDisplayNameAttrib { get; set; }

            public string HasNoAttrib { get; set; }
        }
        #endregion
    }
}