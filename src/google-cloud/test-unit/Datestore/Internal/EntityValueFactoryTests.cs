using System;
using System.Collections.Generic;
using FakeItEasy;
using Google.Cloud.Datastore.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using RapidCore;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Datastore.Internal;
using Xunit;

namespace unittests.Datestore.Internal
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
            Assert.Equal("Cannot build an entity Value without a property\nParameter name: prop", actual.Message);
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
        public void Char()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Char"), entityFactory, new List<string>());
            
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
        public void Int()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Int"), entityFactory, new List<string>());
            
            Assert.Equal(-5, actual.IntegerValue);
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
        public void Float()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Float"), entityFactory, new List<string>());
            
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
        public void Decimal()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("Decimal"), entityFactory, new List<string>());
            
            Assert.Equal(1.123456789012345, actual.DoubleValue);
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
        public void DateTimeX()
        {
            poco.DateTime = DateTime.UtcNow;
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("DateTime"), entityFactory, new List<string>());
            
            Assert.Equal(Timestamp.FromDateTime(poco.DateTime), actual.TimestampValue);
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
        public void TimeSpanX()
        {
            poco.TimeSpan = TimeSpan.FromDays(1.34567);
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("TimeSpan"), entityFactory, new List<string>());
            
            Assert.Equal(poco.TimeSpan.Ticks, actual.IntegerValue);
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
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ArrayString()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ArrayString"), entityFactory, new List<string>());

            Assert.Equal(3, actual.ArrayValue.Values.Count);
            Assert.Equal("one", actual.ArrayValue.Values[0].StringValue);
            Assert.Equal("two", actual.ArrayValue.Values[1].StringValue);
            Assert.Equal("three", actual.ArrayValue.Values[2].StringValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
        }
        
        [Fact]
        public void ArrayInt()
        {
            var actual = EntityValueFactory.FromPropertyInfo(poco, poco.GetType().GetProperty("ArrayInt"), entityFactory, new List<string>());

            Assert.Equal(3, actual.ArrayValue.Values.Count);
            Assert.Equal(1, actual.ArrayValue.Values[0].IntegerValue);
            Assert.Equal(2, actual.ArrayValue.Values[1].IntegerValue);
            Assert.Equal(3, actual.ArrayValue.Values[2].IntegerValue);
            Assert.Equal(true, actual.ExcludeFromIndexes);
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

        [Fact]
        public void ProtectAgainstInfiniteLoops()
        {
            // it has to real, in order to properly get the recursion tested
            var realEntityFactory = new ReflectionBasedEntityFactory(new DatastoreReflector());
            
            // infinite recursion... ouch
            var a = new Infinite();
            var b = new Infinite();
            a.Other = b;
            b.Other = a;
            poco.Infinite = a;
            
            // deep nesting, which is ok
            // it is here to show, that we are "resetting" for every top-level property
            var n1 = new Infinite();
            var n2 = new Infinite {Other = n1};
            var n3 = new Infinite {Other = n2};
            var n4 = new Infinite {Other = n3};
            var n5 = new Infinite {Other = n4};
            var n6 = new Infinite {Other = n5};
            var n7 = new Infinite {Other = n6};
            var n8 = new Infinite {Other = n7};
            var n9 = new Infinite {Other = n8};
            var n10 = new Infinite {Other = n9};
            poco.Nesting = n10;

            var actual = Record.Exception(() => EntityValueFactory.FromPropertyInfo(
                poco,
                poco.GetType().GetProperty("Infinite"),
                realEntityFactory,
                new List<string>()
            ));

            Assert.IsType<RecursionException>(actual);
            Assert.Equal($"Recursion depth has reached 20 - bailing out.{Environment.NewLine}Path: Infinite.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other", actual.Message);
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
            public decimal Decimal => 1.123456789012345m;
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
            public Infinite Infinite { get; set; }
            public Infinite Nesting { get; set; }

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
        
        public class Infinite
        {
            public Infinite Other { get; set; }
        }
        #endregion
    }
}