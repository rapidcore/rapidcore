using System;
using System.Collections.Generic;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class InsertAndLoadTests : DatastoreConnectionTestBase
    {
        private readonly Random random;

        public InsertAndLoadTests()
        {
            random = new Random();
        }

        #region Insert
        [Fact]
        public async void CanInsert()
        {
            EnsureEmptyKind("DasPoco");
            
            var n1 = new Nested();
            var n2 = new Nested {Other = n1};
            var n3 = new Nested {Other = n2};
            var n4 = new Nested {Other = n3};

            var poco = new DasPoco
            {
                Id = Guid.NewGuid(),
                Complex = new Sub(),
                Nesting = n4
            };

            await connection.InsertAsync("DasPoco", poco);
        }
        #endregion

        #region Get by id
        [Fact]
        public async void GetByIdAsync_long()
        {
            EnsureEmptyKind("NumericIdPoco");
            
            var poco = new NumericIdPoco
            {
                Id = random.Next(1, int.MaxValue),
                String = Guid.NewGuid().ToString()
            };

            await connection.InsertAsync(poco);

            var actual = await connection.GetByIdAsync<NumericIdPoco>(poco.Id);
            
            Assert.Equal(poco.Id, actual.Id);
            Assert.Equal(poco.String, actual.String);
        }
        
        [Fact]
        public async void GetByIdAsync_long_kind()
        {
            EnsureEmptyKind("GetByIdAsync_long_kind");
            
            var poco = new NumericIdPoco
            {
                Id = random.Next(1, int.MaxValue),
                String = Guid.NewGuid().ToString()
            };

            await connection.InsertAsync("GetByIdAsync_long_kind", poco);

            var actual = await connection.GetByIdAsync<NumericIdPoco>(poco.Id, "GetByIdAsync_long_kind");
            
            Assert.Equal(poco.Id, actual.Id);
            Assert.Equal(poco.String, actual.String);
        }
        
        [Fact]
        public async void GetByIdAsync_string()
        {
            EnsureEmptyKind("StringIdPoco");
            
            var poco = new StringIdPoco
            {
                Id = Guid.NewGuid().ToString(),
                String = Guid.NewGuid().ToString()
            };

            await connection.InsertAsync(poco);

            var actual = await connection.GetByIdAsync<StringIdPoco>(poco.Id);
            
            Assert.Equal(poco.Id, actual.Id);
            Assert.Equal(poco.String, actual.String);
        }
        
        [Fact]
        public async void GetByIdAsync_string_kind()
        {
            EnsureEmptyKind("GetByIdAsync_string_kind");
            
            var poco = new StringIdPoco
            {
                Id = Guid.NewGuid().ToString(),
                String = Guid.NewGuid().ToString()
            };

            await connection.InsertAsync("GetByIdAsync_string_kind", poco);

            var actual = await connection.GetByIdAsync<StringIdPoco>(poco.Id, "GetByIdAsync_string_kind");
            
            Assert.Equal(poco.Id, actual.Id);
            Assert.Equal(poco.String, actual.String);
        }
        #endregion
        
        #region POCOs
        public class DasPoco
        {
            public Guid Id { get; set; }
            public bool True => true;
            public bool False => false;
            public char Char => 'c';
            public string String => "string";
            public byte Byte => 5;
            public short Short => -5;
            public int Int => -5;
            public long Long => -5L;
            public float Float => 1.2f;
            public double Double => 1.2;
            public decimal Decimal => 1.123456789012345m;
            public DasPocoEnum Enum => DasPocoEnum.Two;
            public DateTime DateTime { get; set; } = DateTime.UtcNow;
            public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
            public TimeSpan TimeSpan { get; set; } = TimeSpan.FromDays(1.34656);
            public List<string> ListString { get; set; } = new List<string> { "one", "two", "three" };
            public string[] ArrayString { get; set; } = {"one", "two", "three"};
            public int[] ArrayInt { get; set; } = {1, 2, 3};
            public byte[] Binary { get; set; } = new byte[] {1, 2, 3, 4, 5};
            public string Null => null;
            public Sub Complex { get; set; }
            public Nested Nesting { get; set; }

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
        
        public class Nested
        {
            public Nested Other { get; set; }
        }
        
        public class NumericIdPoco
        {
            public long Id { get; set; }
            public string String { get; set; }
        }
        
        public class StringIdPoco
        {
            public string Id { get; set; }
            public string String { get; set; }
        }
        #endregion
    }
}