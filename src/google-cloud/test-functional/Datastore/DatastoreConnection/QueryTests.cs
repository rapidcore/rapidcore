using System;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Testing;
using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class QueryTests : DatastoreConnectedTestBase
    {
        private readonly RapidCore.GoogleCloud.Datastore.DatastoreConnection connection;

        public QueryTests()
        {
            connection = new RapidCore.GoogleCloud.Datastore.DatastoreConnection(GetDb());
        }

        #region Prepare data
        private void PrepareData(string kind)
        {
            EnsureEmptyKind(kind);
            
            Insert(new Entity
            {
                Key = GetKey(kind, 1),
                ["String"] = "one",
                ["X"] = 3
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 2),
                ["String"] = "two",
                ["X"] = 3
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 3),
                ["String"] = "three",
                ["X"] = 5
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 4),
                ["String"] = "four",
                ["X"] = 6
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 5),
                ["String"] = "five",
                ["X"] = 4
            });
        }
        #endregion

        [Fact]
        public async void Query_query_withKindSet()
        {
            PrepareData("DonkeyDiamonds");
            
            var actual = await connection.Query<Full>(new Query("DonkeyDiamonds")
            {
                Filter = Filter.LessThanOrEqual("X", new Value {IntegerValue = 4})
            });
            
            Assert.Equal(3, actual.Count);
            Assert.Contains(actual, x => x.Id == 1 && x.String == "one");
            Assert.Contains(actual, x => x.Id == 2 && x.String == "two");
            Assert.Contains(actual, x => x.Id == 5 && x.String == "five");
        }
        
        [Fact]
        public async void Query_query_withoutSettingKind()
        {
            PrepareData("Full");
            
            var actual = await connection.Query<Full>(new Query
            {
                Filter = Filter.LessThanOrEqual("X", new Value {IntegerValue = 4})
            });
            
            Assert.Equal(3, actual.Count);
            Assert.Contains(actual, x => x.Id == 1 && x.String == "one");
            Assert.Contains(actual, x => x.Id == 2 && x.String == "two");
            Assert.Contains(actual, x => x.Id == 5 && x.String == "five");
        }
        
        [Fact]
        public async void Query_query_kind()
        {
            PrepareData("BagOfBaguettes");
            
            var actual = await connection.Query<Full>(new Query
            {
                Filter = Filter.LessThanOrEqual("X", new Value {IntegerValue = 4})
            }, "BagOfBaguettes");
            
            Assert.Equal(3, actual.Count);
            Assert.Contains(actual, x => x.Id == 1 && x.String == "one");
            Assert.Contains(actual, x => x.Id == 2 && x.String == "two");
            Assert.Contains(actual, x => x.Id == 5 && x.String == "five");
        }
        
        [Fact]
        public async void Query_gql()
        {
            PrepareData("BagOfBaguettes");
            
            var actual = await connection.Query<Full>(new GqlQuery
            {
                AllowLiterals = true,
                QueryString = "select * from BagOfBaguettes where X<=4"
            });
            
            Assert.Equal(3, actual.Count);
            Assert.Contains(actual, x => x.Id == 1 && x.String == "one");
            Assert.Contains(actual, x => x.Id == 2 && x.String == "two");
            Assert.Contains(actual, x => x.Id == 5 && x.String == "five");
        }

        #region POCOs
        public class Full
        {
            public int Id { get; set; }
            public string String { get; set; }
            public int X { get; set; }
        }
        #endregion
    }
}