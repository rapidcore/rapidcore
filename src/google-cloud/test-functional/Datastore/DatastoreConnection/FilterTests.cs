using System;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class FilterTests:DatastoreConnectionTestBase
    {
        #region Prepare data
     
        private void PrepareData(string kind)
        {
            EnsureEmptyKind(kind);
            
            Insert(new Entity
            {
                Key = GetKey(kind, 1),
                ["Value"] = "one",
                ["X"] = 4,
                ["BOO"]=true,
                ["LValue"]=2L,
                ["DATE"]=new DateTime(2010,10,01).ToUniversalTime()
                    
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 2),
                ["Value"] = "two",
                ["X"] = 3,
                ["BOO"]=true,
                ["LValue"]=2L,
                ["DATE"]=new DateTime(2010,10,01).ToUniversalTime()
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 3),
                ["Value"] = "three",
                ["X"] = 5,
                ["BOO"]=false,
                ["LValue"]=2L,
                ["DATE"]=new DateTime(2015,10,01).ToUniversalTime()
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 4),
                ["Value"] = "four",
                ["X"] = 6,
                ["BOO"]=true,
                ["LValue"]=2L,
                ["DATE"]=new DateTime(2014,10,01).ToUniversalTime()
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 5),
                ["Value"] = "five",
                ["X"] = 4,
                ["BOO"]=false,
                ["LValue"]=2L,
                ["DATE"]=new DateTime(2013,10,01).ToUniversalTime()
            });
            
            Insert(new Entity
            {
                Key = GetKey(kind, 6),
                ["Value"] = "five",
                ["X"] = 42,
                ["BOO"]=true,
                ["LValue"]=3L,
                ["DATE"]=new DateTime(2012,10,01).ToUniversalTime()
            });
        }
        #endregion
    
        [Fact]
        public async void Filter_EQUAL_WithoutKindSetting()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>( first => first.Value == "five" && first.X==4);
                Assert.Equal(1,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_EQUAL_WithKind()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("DiffKind");

                var actual = await connection.FilterAsync<Dif>("DiffKind", first => first.Value == "five" && first.X==4);
                Assert.Equal(1,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_GREATER_THAN()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>( first => first.Value == "five" && first.X > 4);
                Assert.Equal(1,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_GREATER_THAN_OR_EQUAL()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>( first => first.Value == "five" && first.X >= 4);
                Assert.Equal(2,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_LESS_THAN()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>( first => 
                    first.Value == "five" && first.X < 6 );
                Assert.Equal(1,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_LESS_THAN_OR_EQUAL()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>( first=> first.X <= 6);
                Assert.Equal(5,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_EQUAL_BOOL()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>( first=> first.BOO == false);
                Assert.Equal(2,actual.Count);
                
            });
        }
        [Fact]
        public async void Filter_EQUAL_LONG()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Dif");

                var actual = await connection.FilterAsync<Dif>(first=> first.LValue > 2L);
                Assert.Equal(1,actual.Count);
                
            });
        }
      
    
        #region POCOs
        public class Dif
        {
            
            public int Id { get; set; }
            public string Value { get; set; }
            public int X { get; set; }
            public bool BOO { get; set; }
            public long LValue { get; set; }
          
            public DateTime DATE { get; set; }
            
        }
        
        #endregion
    }
}