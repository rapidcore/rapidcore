using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class UpdateTests : DatastoreConnectionTestBase
    {
        [Fact]
        public async void Update_without_kind()
        {
            EnsureEmptyKind("TheUpdater");
            
            var poco = new TheUpdater
            {
                Id = 666,
                String = "Miav",
                X = 333
            };

            await connection.InsertAsync(poco);

            poco.X = 999;
            await connection.UpdateAsync(poco);

            var firstAll = GetAll("TheUpdater");
            
            Assert.Equal(1, firstAll.Count);
            Assert.Equal(666, firstAll[0].Key.Path[0].Id);
            Assert.Equal("Miav", firstAll[0]["String"].StringValue);
            Assert.Equal(999, firstAll[0]["X"].IntegerValue);
        }
        
        [Fact]
        public async void Update_with_kind()
        {
            EnsureEmptyKind("DeviledPigs");
            
            var poco = new TheUpdater
            {
                Id = 666,
                String = "Oink",
                X = 333
            };

            await connection.InsertAsync("DeviledPigs", poco);

            poco.X = 999;
            await connection.UpdateAsync(poco, "DeviledPigs");
            
            var firstAll = GetAll("DeviledPigs");
            
            Assert.Equal(1, firstAll.Count);
            Assert.Equal(666, firstAll[0].Key.Path[0].Id);
            Assert.Equal("Oink", firstAll[0]["String"].StringValue);
            Assert.Equal(999, firstAll[0]["X"].IntegerValue);
        }
        
        #region POCOs
        public class TheUpdater
        {
            public int Id { get; set; }
            public string String { get; set; }
            public int X { get; set; }
        }
        #endregion
    }
}