using System;
using System.Collections.Generic;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class TypeNullableExtensionsTests
    {
        [Theory]
        // no
        [InlineData(typeof(bool), false)]
        [InlineData(typeof(byte), false)]
        [InlineData(typeof(sbyte), false)]
        [InlineData(typeof(short), false)]
        [InlineData(typeof(ushort), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(uint), false)]
        [InlineData(typeof(long), false)]
        [InlineData(typeof(ulong), false)]
        [InlineData(typeof(float), false)]
        [InlineData(typeof(double), false)]
        [InlineData(typeof(decimal), false)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(DateTime), false)]
        [InlineData(typeof(DateTimeOffset), false)]
        [InlineData(typeof(TimeSpan), false)]
        [InlineData(typeof(int[]), false)]
        [InlineData(typeof(List<string>), false)]
        [InlineData(typeof(SomeClass), false)]
        [InlineData(typeof(Type), false)]
        // yes
        [InlineData(typeof(bool?), true)]
        [InlineData(typeof(DateTime?), true)]
        public void IsNullable(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsNullable());
        }
        
        [Theory]
        [InlineData(typeof(bool?), typeof(bool))]
        [InlineData(typeof(byte?), typeof(byte))]
        [InlineData(typeof(sbyte?), typeof(sbyte))]
        [InlineData(typeof(short?), typeof(short))]
        [InlineData(typeof(ushort?), typeof(ushort))]
        [InlineData(typeof(int?), typeof(int))]
        [InlineData(typeof(uint?), typeof(uint))]
        [InlineData(typeof(long?), typeof(long))]
        [InlineData(typeof(ulong?), typeof(ulong))]
        [InlineData(typeof(float?), typeof(float))]
        [InlineData(typeof(double?), typeof(double))]
        [InlineData(typeof(decimal?), typeof(decimal))]
        [InlineData(typeof(char?), typeof(char))]
        [InlineData(typeof(DateTime?), typeof(DateTime))]
        [InlineData(typeof(DateTimeOffset?), typeof(DateTimeOffset))]
        [InlineData(typeof(TimeSpan?), typeof(TimeSpan))]
        public void GetNullableType(Type type, Type expected)
        {
            Assert.Equal(expected, type.GetUnderlyingNullableType());
        }
        
        [Theory]
        // not nullable
        [InlineData(typeof(bool), typeof(bool))]
        [InlineData(typeof(byte), typeof(byte))]
        [InlineData(typeof(sbyte), typeof(sbyte))]
        [InlineData(typeof(short), typeof(short))]
        [InlineData(typeof(ushort), typeof(ushort))]
        [InlineData(typeof(int), typeof(int))]
        [InlineData(typeof(uint), typeof(uint))]
        [InlineData(typeof(long), typeof(long))]
        [InlineData(typeof(ulong), typeof(ulong))]
        [InlineData(typeof(float), typeof(float))]
        [InlineData(typeof(double), typeof(double))]
        [InlineData(typeof(decimal), typeof(decimal))]
        [InlineData(typeof(char), typeof(char))]
        [InlineData(typeof(DateTime), typeof(DateTime))]
        [InlineData(typeof(DateTimeOffset), typeof(DateTimeOffset))]
        [InlineData(typeof(TimeSpan), typeof(TimeSpan))]
        [InlineData(typeof(int[]), typeof(int[]))]
        [InlineData(typeof(List<string>), typeof(List<string>))]
        [InlineData(typeof(SomeClass), typeof(SomeClass))]
        [InlineData(typeof(Type), typeof(Type))]
        // nullable
        [InlineData(typeof(bool?), typeof(bool))]
        [InlineData(typeof(byte?), typeof(byte))]
        [InlineData(typeof(sbyte?), typeof(sbyte))]
        [InlineData(typeof(short?), typeof(short))]
        [InlineData(typeof(ushort?), typeof(ushort))]
        [InlineData(typeof(int?), typeof(int))]
        [InlineData(typeof(uint?), typeof(uint))]
        [InlineData(typeof(long?), typeof(long))]
        [InlineData(typeof(ulong?), typeof(ulong))]
        [InlineData(typeof(float?), typeof(float))]
        [InlineData(typeof(double?), typeof(double))]
        [InlineData(typeof(decimal?), typeof(decimal))]
        [InlineData(typeof(char?), typeof(char))]
        [InlineData(typeof(DateTime?), typeof(DateTime))]
        [InlineData(typeof(DateTimeOffset?), typeof(DateTimeOffset))]
        [InlineData(typeof(TimeSpan?), typeof(TimeSpan))]
        public void GetTypeOrNullableType(Type type, Type expected)
        {
            Assert.Equal(expected, type.GetTypeOrUnderlyingNullableType());
        }

        #region POCOs
        public class SomeClass
        {
        }
        #endregion
    }
}