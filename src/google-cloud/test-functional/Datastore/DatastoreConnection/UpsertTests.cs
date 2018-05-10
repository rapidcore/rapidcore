using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class UpsertTests : DatastoreConnectionTestBase
    {
        [Fact]
        public async void Upsert_without_kind()
        {
            EnsureEmptyKind("TheUpserter");
            
            var poco = new TheUpserter
            {
                Id = 666,
                String = "Miav",
                X = 333
            };

            await connection.UpsertAsync(poco);

            var firstAll = GetAll("TheUpserter");
            
            Assert.Equal(1, firstAll.Count);
            Assert.Equal(666, firstAll[0].Key.Path[0].Id);
            Assert.Equal("Miav", firstAll[0]["String"].StringValue);
            Assert.Equal(333, firstAll[0]["X"].IntegerValue);

            poco.X = 999;
            await connection.UpsertAsync(poco);
            
            var secondAll = GetAll("TheUpserter");
            
            Assert.Equal(1, secondAll.Count);
            Assert.Equal(666, secondAll[0].Key.Path[0].Id);
            Assert.Equal("Miav", secondAll[0]["String"].StringValue);
            Assert.Equal(999, secondAll[0]["X"].IntegerValue);
        }
        
        [Fact]
        public async void Upsert_with_kind()
        {
            EnsureEmptyKind("KittensAreEvil");
            
            var poco = new TheUpserter
            {
                Id = 666,
                String = "Miav",
                X = 333
            };

            await connection.UpsertAsync(poco, "KittensAreEvil");

            var firstAll = GetAll("KittensAreEvil");
            
            Assert.Equal(1, firstAll.Count);
            Assert.Equal(666, firstAll[0].Key.Path[0].Id);
            Assert.Equal("Miav", firstAll[0]["String"].StringValue);
            Assert.Equal(333, firstAll[0]["X"].IntegerValue);

            poco.X = 999;
            await connection.UpsertAsync(poco, "KittensAreEvil");
            
            var secondAll = GetAll("KittensAreEvil");
            
            Assert.Equal(1, secondAll.Count);
            Assert.Equal(666, secondAll[0].Key.Path[0].Id);
            Assert.Equal("Miav", secondAll[0]["String"].StringValue);
            Assert.Equal(999, secondAll[0]["X"].IntegerValue);
        }
        
        #region POCOs
        public class TheUpserter
        {
            public int Id { get; set; }
            public string String { get; set; }
            public int X { get; set; }
        }
        #endregion
    }
}