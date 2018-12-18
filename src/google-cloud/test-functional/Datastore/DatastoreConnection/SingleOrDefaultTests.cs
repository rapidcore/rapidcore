using System;
using Google.Cloud.Datastore.V1;
using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class SingleOrDefaultTests : DatastoreConnectionTestBase
    {
        #region Prepare data

        private void PrepareData(string kind)
        {
            EnsureEmptyKind(kind);

            Insert(new Entity
            {
                Key = GetKey(kind, 1),
                ["Value"] = 2
            });

            Insert(new Entity
            {
                Key = GetKey(kind, 2),
                ["Value"] = 3
            });

            Insert(new Entity
            {
                Key = GetKey(kind, 3),
                ["Value"] = 5
            });

            Insert(new Entity
            {
                Key = GetKey(kind, 4),
                ["Value"] = 6
            });

            Insert(new Entity
            {
                Key = GetKey(kind, 5),
                ["Value"] = 4
            });
        }

        #endregion

        [Fact]
        public async void SingleOrDefault_QueryWithKindSet()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("HighBaggageKind");

                var actual = await connection.SingleOrDefaultAsync<Baggage>(new Query("HighBaggageKind")
                {
                    Filter = Filter.GreaterThan("Value", new Value {IntegerValue = 5})
                });

                Assert.Equal(6,actual.Value);
                Assert.Equal(4,actual.Id);
            });
        }
        [Fact]
        public async void SingleOrDefault_query_withoutSettingKind()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("Baggage");

                var actual = await connection.SingleOrDefaultAsync<Baggage>(new Query
                {
                    Filter = Filter.GreaterThan("Value", new Value {IntegerValue = 5})
                });

                Assert.Equal(6, actual.Value);
                Assert.Equal(4,actual.Id);
            });
        }
       
        [Fact]
        public async void SingleOrDefault_Query_Kind()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("HighBaggage");

                var actual = await connection.SingleOrDefaultAsync<Baggage>(new Query
                {
                    Filter = Filter.GreaterThan("Value", new Value {IntegerValue = 5})
                }, "HighBaggage");


                Assert.Equal(6,actual.Value);
                Assert.Equal(4,actual.Id);
            });
        }
        [Fact]
        public async void SingleOrDefault_Query_Exception()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("HighBaggageError");
                var actual = await Record.ExceptionAsync(async () =>
                    await connection.SingleOrDefaultAsync<Baggage>(new Query
                    {
                        Filter = Filter.GreaterThan("Value", new Value {IntegerValue = 3})
                    }, "HighBaggageError"));

                Assert.IsType<InvalidOperationException>(actual);
                Assert.Equal("Sequence contains more than one element", actual.Message);
            });
        }

        [Fact]
        public async void SingleOrDefault_GqlQuery()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("SmallBaggage");

                var actual = await connection.SingleOrDefaultAsync<Baggage>(new GqlQuery
                {
                    AllowLiterals = true,
                    QueryString = "select * from SmallBaggage where Value<3"
                });

                Assert.Equal(2,actual.Value);
                Assert.Equal(1,actual.Id);
            });
        }

        [Fact]
        public async void SingleOrDefault_GqlQuery_Exception()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("SmallBaggageError");
                var actual = await Record.ExceptionAsync(async () =>
                    await connection.SingleOrDefaultAsync<Baggage>(new GqlQuery
                    {
                        AllowLiterals = true,
                        QueryString = "select * from SmallBaggageError where Value>3"
                    }));

                Assert.IsType<InvalidOperationException>(actual);
                Assert.Equal("Sequence contains more than one element", actual.Message);
            });
        }
        
        [Fact]
        public async void SingleOrDefault_Filter()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("SmallBaggageFilter");

                var actual = await connection.SingleOrDefaultAsync<Baggage>("SmallBaggageFilter",x=>x.Value<3);

                Assert.Equal(2,actual.Value);
                Assert.Equal(1,actual.Id);
            });
        }

        [Fact]
        public async void SingleOrDefault_Filter_Exception()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                PrepareData("SmallBaggageFault");
                var actual = await Record.ExceptionAsync(async () =>
                    await connection.SingleOrDefaultAsync<Baggage>("SmallBaggageFault",x=>x.Value>3));

                Assert.IsType<InvalidOperationException>(actual);
                Assert.Equal("Sequence contains more than one element", actual.Message);
            });
        }

        #region POCOs

        public class Baggage
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }

        #endregion

    }
}



   