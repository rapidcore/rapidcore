using System;
using System.ComponentModel;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class PropertyInfoAttributesExtensionsTests
    {
        [Theory]
        [InlineDataAttribute("HasObsoleteAttrib", typeof(ObsoleteAttribute), true)] // returns true when it has the attribute
        [InlineDataAttribute("HasObsoleteAttrib", typeof(DisplayNameAttribute), false)] // returns false when it does not have the attribute, although it has another attribute
        [InlineDataAttribute("HasNoAttrib", typeof(DisplayNameAttribute), false)] // returns false when it does not have the attribute
        public void HasAttribute(string propertyName, Type attributeType, bool expected)
        {
            Assert.Equal(expected, GetProperty(propertyName).HasAttribute(attributeType));
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

        private PropertyInfo GetProperty(string propertyName)
        {
            return typeof(GuineaPig).GetTypeInfo().GetProperty(propertyName);
        }

        #region GuineaPig
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