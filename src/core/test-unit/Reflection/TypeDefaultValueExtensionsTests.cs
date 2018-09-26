using System;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class TypeDefaultValueExtensionsTests
    {
        [Fact]
        public void GetDefaultValue_string()
        {
            Assert.Equal(default(string), typeof(string).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_char()
        {
            Assert.Equal(default(char), typeof(char).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_bool()
        {
            Assert.Equal(default(bool), typeof(bool).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_byte()
        {
            Assert.Equal(default(byte), typeof(byte).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_short()
        {
            Assert.Equal(default(short), typeof(short).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_ushort()
        {
            Assert.Equal(default(ushort), typeof(ushort).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_int()
        {
            Assert.Equal(default(int), typeof(int).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_uint()
        {
            Assert.Equal(default(uint), typeof(uint).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_long()
        {
            Assert.Equal(default(long), typeof(long).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_ulong()
        {
            Assert.Equal(default(ulong), typeof(ulong).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_float()
        {
            Assert.Equal(default(float), typeof(float).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_double()
        {
            Assert.Equal(default(double), typeof(double).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_decimal()
        {
            Assert.Equal(default(decimal), typeof(decimal).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_array()
        {
            Assert.Equal(default(int[]), typeof(int[]).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_class()
        {
            Assert.Equal(default(ClassyVictim), typeof(ClassyVictim).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_struct()
        {
            Assert.Equal(default(StructyVictim), typeof(StructyVictim).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_nullable_int()
        {
            Assert.Equal(default(int?), typeof(int?).GetDefaultValue());
        }
        
        [Fact]
        public void GetDefaultValue_generic_class()
        {
            Assert.Equal(default(GenericVictim<ClassyVictim>), typeof(GenericVictim<ClassyVictim>).GetDefaultValue());
        }

        #region Victims
        public class ClassyVictim { }
        public struct StructyVictim { }
        public class GenericVictim<T> { }
        #endregion
    }
}