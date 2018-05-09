using System;
using System.Collections.Generic;
using RapidCore.Environment;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Testing;
using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class InsertAsyncTests : DatastoreConnectedTestBase
    {
        private readonly RapidCore.GoogleCloud.Datastore.DatastoreConnection connection;

        public InsertAsyncTests()
        {
            var envVars = new EnvironmentVariables();
            ConnectionString = envVars.Get("DATASTORE_URL", "http://localhost:8081");
            connection = new RapidCore.GoogleCloud.Datastore.DatastoreConnection(GetDb());
        }

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
        #endregion
    }
}