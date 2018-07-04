using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FakeItEasy;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Datastore.ReflectionBased;
using RapidCore.GoogleCloud.Datastore.ReflectionBased.Internal;
using Xunit;
using Value = Google.Cloud.Datastore.V1.Value;

namespace unittests.Datestore.ReflectionBased.Internal
{
    public class PocoValueFactoryTests
    {
        /**
         * I know it looks a bit funky in the nullable_hasvalue tests,
         * that we are asserting that the type is the underlying type and
         * not the nullable. It is because the runtime is handling the nullable
         * for us.
         * https://blogs.msdn.microsoft.com/haibo_luo/2005/08/23/reflection-and-nullablet/
         */
        
        private readonly DasPoco poco;
        private readonly IEntityFactory entityFactory;
        private readonly IPocoFactory pocoFactory;

        public PocoValueFactoryTests()
        {
            entityFactory = new ReflectionBasedEntityFactory(new DatastoreReflector());
            pocoFactory = A.Fake<IPocoFactory>(o => o.Strict());
            poco = new DasPoco();
        }

        [Fact]
        public void NullPropertyInfoNotAllowed()
        {
            var actual = Record.Exception(() => PocoValueFactory.FromEntityValue(null, new Value(), pocoFactory));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot extract the proper data from the Value without having the property{Environment.NewLine}Parameter name: prop", actual.Message);
        }
        
        [Fact]
        public void NullValueNotAllowed()
        {
            var actual = Record.Exception(() => PocoValueFactory.FromEntityValue(poco.GetType().GetProperty("True"), null, pocoFactory));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot extract data from a null Value{Environment.NewLine}Parameter name: value", actual.Message);
        }

        [Fact]
        public void BoolTrue()
        {
            var prop = poco.GetType().GetProperty("True");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<bool>(actual);
            Assert.Equal(true, actual);
        }
        
        [Fact]
        public void BoolFalse()
        {
            var prop = poco.GetType().GetProperty("False");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<bool>(actual);
            Assert.Equal(false, actual);
        }
        
        [Fact]
        public void BoolNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("BoolNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void BoolNullable_hasValue()
        {
            poco.BoolNullable = true;
            var prop = poco.GetType().GetProperty("BoolNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<bool>(actual);
            Assert.Equal(true, typedActual);
        }
        
        [Fact]
        public void Char()
        {
            var prop = poco.GetType().GetProperty("Char");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<char>(actual);
            Assert.Equal('c', actual);
        }
        
        [Fact]
        public void CharNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("CharNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void CharNullable_hasValue()
        {
            poco.CharNullable = 'x';
            var prop = poco.GetType().GetProperty("CharNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<char>(actual);
            Assert.Equal('x', typedActual);
        }
        
        [Fact]
        public void Byte()
        {
            var prop = poco.GetType().GetProperty("Byte");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<byte>(actual);
            Assert.Equal((byte)5, actual);
        }
        
        [Fact]
        public void ByteNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("ByteNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void ByteNullable_hasValue()
        {
            poco.ByteNullable = 17;
            var prop = poco.GetType().GetProperty("ByteNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<byte>(actual);
            Assert.Equal(17, typedActual);
        }
        
        [Theory]
        [InlineData("Sbyte", typeof(sbyte))]
        [InlineData("Ushort", typeof(ushort))]
        [InlineData("Uint", typeof(uint))]
        [InlineData("Ulong", typeof(ulong))]
        public void ThrowOnUnsupportedTypes(string propertyName, System.Type type)
        {
            var actual = Record.Exception(() => PocoValueFactory.FromEntityValue(poco.GetType().GetProperty(propertyName), new Value(), pocoFactory));

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal($"The type {type.Name} is not supported", actual.Message);
        }
        
        [Fact]
        public void Short()
        {
            var prop = poco.GetType().GetProperty("Short");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<short>(actual);
            Assert.Equal((short)-5, actual);
        }
        
        [Fact]
        public void ShortNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("ShortNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void ShortNullable_hasValue()
        {
            poco.ShortNullable = -5;
            var prop = poco.GetType().GetProperty("ShortNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<short>(actual);
            Assert.Equal(-5, typedActual);
        }
        
        [Fact]
        public void Int()
        {
            var prop = poco.GetType().GetProperty("Int");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<int>(actual);
            Assert.Equal(-5, actual);
        }
        
        [Fact]
        public void IntNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("IntNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void IntNullable_hasValue()
        {
            poco.IntNullable = 789;
            var prop = poco.GetType().GetProperty("IntNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<int>(actual);
            Assert.Equal(789, typedActual);
        }
        
        [Fact]
        public void Long()
        {
            var prop = poco.GetType().GetProperty("Long");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<long>(actual);
            Assert.Equal((long)-5, actual);
        }
        
        [Fact]
        public void LongNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("LongNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void LongNullable_hasValue()
        {
            poco.LongNullable = 789456123;
            var prop = poco.GetType().GetProperty("LongNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<long>(actual);
            Assert.Equal(789456123, typedActual);
        }
        
        [Fact]
        public void Float()
        {
            var prop = poco.GetType().GetProperty("Float");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<float>(actual);
            Assert.Equal((float)1.2, actual);
        }
        
        [Fact]
        public void FloatNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("FloatNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void FloatNullable_hasValue()
        {
            poco.FloatNullable = 1.2f;
            var prop = poco.GetType().GetProperty("FloatNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<float>(actual);
            Assert.Equal(1.2f, typedActual);
        }
        
        [Fact]
        public void Double()
        {
            var prop = poco.GetType().GetProperty("Double");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<double>(actual);
            Assert.Equal(1.2, actual);
        }
        
        [Fact]
        public void DoubleNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("DoubleNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void DoubleNullable_hasValue()
        {
            poco.DoubleNullable = 1.234;
            var prop = poco.GetType().GetProperty("DoubleNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<double>(actual);
            Assert.Equal(1.234, typedActual);
        }
        
        [Fact]
        public void Decimal()
        {
            var prop = poco.GetType().GetProperty("Decimal");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var actualDec = Assert.IsType<decimal>(actual);
            Assert.Equal(12345.0123456789012345m, actualDec, 15);
        }
        
        [Fact]
        public void DecimalNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("DecimalNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void DecimalNullable_hasValue()
        {
            poco.DecimalNullable = 12345.0123456789012345m;
            var prop = poco.GetType().GetProperty("DecimalNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<decimal>(actual);
            Assert.Equal(12345.0123456789012345m, typedActual);
        }
        
        [Fact]
        public void Enum()
        {
            var prop = poco.GetType().GetProperty("Enum");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<DasPocoEnum>(actual);
            Assert.Equal(DasPocoEnum.Two, actual);
        }
        
        [Fact]
        public void EnumNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("EnumNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void EnumNullable_hasValue()
        {
            poco.EnumNullable = DasPocoEnum.One;
            var prop = poco.GetType().GetProperty("EnumNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<DasPocoEnum>(actual);
            Assert.Equal(DasPocoEnum.One, typedActual);
        }
        
        [Fact]
        public void DateTimeX()
        {
            poco.DateTime = DateTime.UtcNow;
            
            var prop = poco.GetType().GetProperty("DateTime");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<DateTime>(actual);
            Assert.Equal(poco.DateTime, actual);
        }
        
        [Fact]
        public void DateTimeNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("DateTimeNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void DateTimeNullable_hasValue()
        {
            poco.DateTimeNullable = DateTime.UtcNow;
            var prop = poco.GetType().GetProperty("DateTimeNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<DateTime>(actual);
            Assert.Equal(poco.DateTimeNullable, typedActual);
        }
        
        [Fact]
        public void DateTimeOffsetX()
        {
            poco.DateTimeOffset = DateTimeOffset.UtcNow;
            
            var prop = poco.GetType().GetProperty("DateTimeOffset");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<DateTimeOffset>(actual);
            Assert.Equal(poco.DateTimeOffset, actual);
        }
        
        [Fact]
        public void DateTimeOffsetNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("DateTimeOffsetNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void DateTimeOffsetNullable_hasValue()
        {
            poco.DateTimeOffsetNullable = DateTimeOffset.UtcNow;
            var prop = poco.GetType().GetProperty("DateTimeOffsetNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<DateTimeOffset>(actual);
            Assert.Equal(poco.DateTimeOffsetNullable, typedActual);
        }
        
        [Fact]
        public void TimeSpanX()
        {
            poco.TimeSpan = TimeSpan.FromDays(1.34567);
            
            var prop = poco.GetType().GetProperty("TimeSpan");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<TimeSpan>(actual);
            Assert.Equal(poco.TimeSpan, actual);
        }
        
        [Fact]
        public void TimeSpanNullable_noValue()
        {
            var prop = poco.GetType().GetProperty("TimeSpanNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void TimeSpanNullable_hasValue()
        {
            poco.TimeSpanNullable = TimeSpan.FromHours(789.123);
            var prop = poco.GetType().GetProperty("TimeSpanNullable");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typedActual = Assert.IsType<TimeSpan>(actual);
            Assert.Equal(poco.TimeSpanNullable, typedActual);
        }
        
        [Fact]
        public void ListString()
        {
            var prop = poco.GetType().GetProperty("ListString");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<List<string>>(actual);
            var list = (List<string>) actual;
            Assert.Equal(3, list.Count);
            Assert.Equal("one", list[0]);
            Assert.Equal("two", list[1]);
            Assert.Equal("three", list[2]);
        }
        
        [Fact]
        public void ArrayString()
        {
            var prop = poco.GetType().GetProperty("ArrayString");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<string[]>(actual);
            var list = (string[]) actual;
            Assert.Equal(3, list.Length);
            Assert.Equal("one", list[0]);
            Assert.Equal("two", list[1]);
            Assert.Equal("three", list[2]);
        }
        
        [Fact]
        public void ArrayInt()
        {
            var prop = poco.GetType().GetProperty("ArrayInt");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<int[]>(actual);
            var list = (int[]) actual;
            Assert.Equal(3, list.Length);
            Assert.Equal(1, list[0]);
            Assert.Equal(2, list[1]);
            Assert.Equal(3, list[2]);
        }
        
        [Fact]
        public void Binary()
        {
            var prop = poco.GetType().GetProperty("Binary");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<byte[]>(actual);
            Assert.Equal(poco.Binary, actual);
        }
        
        [Fact]
        public void NullX()
        {
            var prop = poco.GetType().GetProperty("Null");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.Null(actual);
        }
        
        [Fact]
        public void Complex()
        {
            var sub = new Sub();
            poco.Complex = sub;
            
            A.CallTo(() => pocoFactory.FromEmbeddedEntity(typeof(Sub), A<Entity>._)).Returns(sub);
            
            var prop = poco.GetType().GetProperty("Complex");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            Assert.IsType<Sub>(actual);
            Assert.Same(sub, actual);
        }
        
        [Fact]
        public void CustomCollection()
        {
            var simple = new Simple {Hello = "neo"};
            var collection = new MyCollection
            {
                simple
            };

            poco.MyCollection = collection;
            
            A.CallTo(() => pocoFactory.FromEmbeddedEntity(typeof(Simple), A<Entity>._)).Returns(simple);
            
            var prop = poco.GetType().GetProperty("MyCollection");
            var value = EntityValueFactory.FromPropertyInfo(poco, prop, entityFactory, new List<string>());

            var actual = PocoValueFactory.FromEntityValue(prop, value, pocoFactory);

            var typed = Assert.IsType<MyCollection>(actual);
            Assert.Equal(1, typed.Count);
            Assert.Equal("neo", typed[0].Hello);
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
        }
        
        public enum DasPocoEnum
        {
            Zero = 0,
            One = 1,
            Two = 2
        }
        
        public class Sub
        {
            public string SubWhat { get; set; }
            
            public string SubIndexed { get; set; }
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