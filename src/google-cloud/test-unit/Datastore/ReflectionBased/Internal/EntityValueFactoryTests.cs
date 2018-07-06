using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FakeItEasy;
using Google.Cloud.Datastore.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Datastore.ReflectionBased.Internal;
using Xunit;

namespace unittests.Datastore.ReflectionBased.Internal
{
    public class EntityValueFactoryTests
    {
        private readonly DasPoco poco;
        private readonly IEntityFactory entityFactory;

        public EntityValueFactoryTests()
        {
            poco = new DasPoco();
            entityFactory = A.Fake<IEntityFactory>(o => o.Strict());
        }
        
        [Fact]
        public void NullPropertyInfoNotAllowed()
        {
            var actual = Record.Exception(() => EntityValueFactory.FromPropertyInfo(poco, null, entityFactory, new List<string>()));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot build an entity Value without a property{Environment.NewLine}Parameter name: prop", actual.Message);
        }

        [Fact]
        public void BoolTrue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("True"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.BooleanValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void BoolFalse()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("False"), entityFactory, new List<string>());
            
            Assert.Equal(false, actual.BooleanValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void BoolNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("BoolNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void BoolNullable_hasValue()
        {
            poco.BoolNullable = true;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("BoolNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.BooleanValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Char()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Char"), entityFactory, new List<string>());
            
            Assert.Equal("c", actual.StringValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void CharNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("CharNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void CharNullable_hasValue()
        {
            poco.CharNullable = 'c';
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("CharNullable"), entityFactory, new List<string>());
            
            Assert.Equal("c", actual.StringValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Byte()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Byte"), entityFactory, new List<string>());
            
            Assert.Equal(5, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ByteNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ByteNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ByteNullable_hasValue()
        {
            poco.ByteNullable = 6;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ByteNullable"), entityFactory, new List<string>());
            
            Assert.Equal(6, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }

        [Fact]
        public void ThrowOnInterface()
        {
            var actual = Record.Exception(() => EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Interface"), entityFactory, new List<string>()));

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal("\"DasPoco.Interface\" has \"IHavePrettyFace\" as type, but that is just an interface and is therefore not supported", actual.Message);
        }
        
        [Fact]
        public void Collection()
        {
            var simple = new Simple();
            poco.MyCollection = new MyCollection { simple };
            
            var simpleEntity = new Entity();

            A.CallTo(() => entityFactory.EmbeddedEntityFromPoco(simple, A<IList<string>>._)).Returns(simpleEntity);
            
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("MyCollection"), entityFactory, new List<string>());
            
            Assert.Equal(1, actual.ArrayValue.Values.Count);
            Assert.Equal(false, actual.ExcludeFromIndexes);
        }
        
        [Theory]
        [InlineData("Sbyte", typeof(sbyte))]
        [InlineData("Ushort", typeof(ushort))]
        [InlineData("Uint", typeof(uint))]
        [InlineData("Ulong", typeof(ulong))]
        public void ThrowOnUnsupportedTypes(string propertyName, System.Type type)
        {
            var actual = Record.Exception(() => EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty(propertyName), entityFactory, new List<string>()));

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal($"The type {type.Name} is not supported", actual.Message);
        }
        
        [Fact]
        public void Short()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Short"), entityFactory, new List<string>());
            
            Assert.Equal(-5, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ShortNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ShortNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ShortNullable_hasValue()
        {
            poco.ShortNullable = -5;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ShortNullable"), entityFactory, new List<string>());
            
            Assert.Equal(-5, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Int()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Int"), entityFactory, new List<string>());
            
            Assert.Equal(-5, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void IntNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("IntNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void IntNullable_hasValue()
        {
            poco.IntNullable = 234;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("IntNullable"), entityFactory, new List<string>());
            
            Assert.Equal(234, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Long()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Long"), entityFactory, new List<string>());
            
            Assert.Equal(-5, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void LongNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("LongNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void LongNullable_hasValue()
        {
            poco.LongNullable = 8L;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("LongNullable"), entityFactory, new List<string>());
            
            Assert.Equal(8L, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Float()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Float"), entityFactory, new List<string>());
            
            Assert.Equal(1.2, actual.DoubleValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void FloatNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("FloatNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void FloatNullable_hasValue()
        {
            poco.FloatNullable = 1.2f;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("FloatNullable"), entityFactory, new List<string>());
            
            Assert.Equal(1.2, actual.DoubleValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Double()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Double"), entityFactory, new List<string>());
            
            Assert.Equal(1.2, actual.DoubleValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DoubleNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DoubleNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DoubleNullable_hasValue()
        {
            poco.DoubleNullable = 1.2;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DoubleNullable"), entityFactory, new List<string>());
            
            Assert.Equal(1.2, actual.DoubleValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Decimal()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Decimal"), entityFactory, new List<string>());

            var integral = actual.EntityValue["integral"];
            var fraction = actual.EntityValue["fraction"];
            
            Assert.Equal(12345, integral.IntegerValue);
            Assert.Equal(true, integral.ExcludeFromIndexes);
            
            Assert.Equal(0.0123456789012345, fraction.DoubleValue);
            Assert.Equal(true, fraction.ExcludeFromIndexes);
            
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DecimalNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DecimalNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DecimalNullable_hasValue()
        {
            poco.DecimalNullable = 12345.0123456789012345m;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DecimalNullable"), entityFactory, new List<string>());
            
            var integral = actual.EntityValue["integral"];
            var fraction = actual.EntityValue["fraction"];
            
            Assert.Equal(12345, integral.IntegerValue);
            Assert.Equal(true, integral.ExcludeFromIndexes);
            
            Assert.Equal(0.0123456789012345, fraction.DoubleValue);
            Assert.Equal(true, fraction.ExcludeFromIndexes);
            
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Enum()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Enum"), entityFactory, new List<string>());
            
            Assert.Equal("Two", actual.StringValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void EnumNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("EnumNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void EnumNullable_hasValue()
        {
            poco.EnumNullable = DasPocoEnum.One;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("EnumNullable"), entityFactory, new List<string>());
            
            Assert.Equal("One", actual.StringValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DateTimeX()
        {
            poco.DateTime = DateTime.UtcNow;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTime"), entityFactory, new List<string>());
            
            Assert.Equal(Timestamp.FromDateTime(poco.DateTime), actual.TimestampValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DateTimeNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTimeNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DateTimeNullable_hasValue()
        {
            poco.DateTimeNullable = DateTime.UtcNow;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTimeNullable"), entityFactory, new List<string>());
            
            Assert.Equal(Timestamp.FromDateTime(poco.DateTimeNullable.Value), actual.TimestampValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DateTimeOffsetX()
        {
            poco.DateTimeOffset = DateTimeOffset.UtcNow;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTimeOffset"), entityFactory, new List<string>());
            
            Assert.Equal(Timestamp.FromDateTimeOffset(poco.DateTimeOffset), actual.TimestampValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DateTimeOffsetNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTimeOffsetNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void DateTimeOffsetNullable_hasValue()
        {
            poco.DateTimeOffsetNullable = DateTimeOffset.UtcNow;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTimeOffsetNullable"), entityFactory, new List<string>());
            
            Assert.Equal(Timestamp.FromDateTimeOffset(poco.DateTimeOffsetNullable.Value), actual.TimestampValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void TimeSpanX()
        {
            poco.TimeSpan = TimeSpan.FromDays(1.34567);
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("TimeSpan"), entityFactory, new List<string>());
            
            Assert.Equal(poco.TimeSpan.Ticks, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void TimeSpanNullable_noValue()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("TimeSpanNullable"), entityFactory, new List<string>());
            
            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void TimeSpanNullable_hasValue()
        {
            poco.TimeSpanNullable = TimeSpan.FromDays(9.876);
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("TimeSpanNullable"), entityFactory, new List<string>());
            
            Assert.Equal(poco.TimeSpanNullable.Value.Ticks, actual.IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ListString()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ListString"), entityFactory, new List<string>());

            Assert.Equal(3, actual.ArrayValue.Values.Count);
            Assert.Equal("one", actual.ArrayValue.Values[0].StringValue);
            Assert.Equal("two", actual.ArrayValue.Values[1].StringValue);
            Assert.Equal("three", actual.ArrayValue.Values[2].StringValue);
            Assert.Equal(false, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ArrayString()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ArrayString"), entityFactory, new List<string>());

            Assert.Equal(3, actual.ArrayValue.Values.Count);
            Assert.Equal("one", actual.ArrayValue.Values[0].StringValue);
            Assert.Equal("two", actual.ArrayValue.Values[1].StringValue);
            Assert.Equal("three", actual.ArrayValue.Values[2].StringValue);
            Assert.Equal(false, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ArrayInt()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ArrayInt"), entityFactory, new List<string>());

            Assert.Equal(3, actual.ArrayValue.Values.Count);
            Assert.Equal(1, actual.ArrayValue.Values[0].IntegerValue);
            Assert.Equal(2, actual.ArrayValue.Values[1].IntegerValue);
            Assert.Equal(3, actual.ArrayValue.Values[2].IntegerValue);
            Assert.Equal(false, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void Binary()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Binary"), entityFactory, new List<string>());

            var expected = ByteString.CopyFrom(poco.Binary, 0, poco.Binary.Length);
            
            Assert.Equal(expected, actual.BlobValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void NullX()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Null"), entityFactory, new List<string>());

            Assert.Equal(true, actual.IsNull);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void SubX()
        {
            var sub = new Sub();
            var subEntity = new Entity();
            poco.Complex = sub;
            A.CallTo(() => entityFactory.EmbeddedEntityFromPoco(sub, A<IList<string>>._)).Returns(subEntity);
            
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Complex"), entityFactory, new List<string>());

            Assert.Equal(true, actual.ExcludeFromIndexes);
            Assert.Same(subEntity, actual.EntityValue);
        }
        
        [Fact]
        public void Indexed()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Indexed"), entityFactory, new List<string>());

            Assert.Equal(false, actual.ExcludeFromIndexes);
        }
        
        #region POCOs
        public class DasPoco
        {
            public bool True => true;
            public bool False => false;
            public char Char => 'c';
            public string String => "string";
            public byte Byte => 5;
            public sbyte Sbyte => -5;
            public short Short => -5;
            public ushort Ushort => 5;
            public int Int => -5;
            public uint Uint => 5;
            public long Long => -5L;
            public ulong Ulong => 5L;
            public float Float => 1.2f;
            public double Double => 1.2;
            public decimal Decimal => 12345.0123456789012345m;
            public DasPocoEnum Enum => DasPocoEnum.Two;
            public DateTime DateTime { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public TimeSpan TimeSpan { get; set; }
            public List<string> ListString { get; set; } = new List<string> { "one", "two", "three" };
            public string[] ArrayString { get; set; } = {"one", "two", "three"};
            public int[] ArrayInt { get; set; } = {1, 2, 3};
            public byte[] Binary { get; set; } = new byte[] {1, 2, 3, 4, 5};
            public string Null => null;
            public Sub Complex { get; set; }
            public IHavePrettyFace Interface { get; set; }
            public MyCollection MyCollection { get; set; }
            
            public bool? BoolNullable { get; set; }
            public char? CharNullable { get; set; }
            public byte? ByteNullable { get; set; }
            public short? ShortNullable { get; set; }
            public int? IntNullable { get; set; }
            public long? LongNullable { get; set; }
            public float? FloatNullable { get; set; }
            public double? DoubleNullable { get; set; }
            public decimal? DecimalNullable { get; set; }
            public DasPocoEnum? EnumNullable { get; set; }
            public DateTime? DateTimeNullable { get; set; }
            public DateTimeOffset? DateTimeOffsetNullable { get; set; }
            public TimeSpan? TimeSpanNullable { get; set; }

            [Index]
            public string Indexed => "indexed";
        }
        
        public enum DasPocoEnum
        {
            Zero = 0,
            One = 1,
            Two = 2
        }
        
        public class Sub
        {
            public string SubWhat => "wattup";
            
            [Index]
            public string SubIndexed => "foxy lady";
        }
        
        public interface IHavePrettyFace
        {
        }
        
        public class MyCollection : Collection<Simple>
        {
        }
        
        public class Simple
        {
            public string Hello { get; set; }
        }
        #endregion
    }
}