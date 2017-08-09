using System;
using FakeItEasy;
using RapidCore.Mongo.Migration;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Migration
{
    public class ConnectionProviderTests
    {
        private readonly ConnectionProvider provider;
        private readonly MongoDbConnection db1;
        private readonly MongoDbConnection db2;

        public ConnectionProviderTests()
        {
            db1 = A.Fake<MongoDbConnection>();
            db2 = A.Fake<MongoDbConnection>();
            
            provider = new ConnectionProvider();
        }

        [Fact]
        public void Default_Throws_ifEmpty()
        {
            var actual = Record.Exception(() => provider.Default());
            
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Exception>(actual);
        }
        
        [Fact]
        public void Default_Throws_ifNoDefaultDefined()
        {
            provider.Add("one", db1, false);
            
            var actual = Record.Exception(() => provider.Default());
            
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Exception>(actual);
        }
        
        [Fact]
        public void Default_Returns_Default()
        {
            provider.Add("one", db1, true);

            var actual = provider.Default();
            
            Assert.Same(db1, actual);
        }
        
        [Fact]
        public void Named_Throws_ifEmpty()
        {
            var actual = Record.Exception(() => provider.Named("DonkeyKong"));
            
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Exception>(actual);
        }
        
        [Fact]
        public void Named_Throws_ifNotDefined()
        {
            provider.Add("one", db1, false);
            provider.Add("two", db2, false);
            
            var actual = Record.Exception(() => provider.Named("NotDefined...NotEvenALittleBit...IMean..Seriously!!"));
            
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Exception>(actual);
        }
        
        [Fact]
        public void Named_Returns()
        {
            provider.Add("one", db1, false);
            provider.Add("two", db2, false);

            var actual = provider.Named("one");
            
            Assert.Same(db1, actual);
        }

        [Fact]
        public void Add_Throws_ifAlreadyDefined()
        {
            provider.Add("one", db1, false);

            var actual = Record.Exception(() => provider.Add("one", db2, false));
            
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Exception>(actual);
        }
        
        [Fact]
        public void Add_default_Throws_ifDefaultHasAlreadyBeenDefined()
        {
            provider.Add("one", db1, true);

            var actual = Record.Exception(() => provider.Add("two", db2, true));
            
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Exception>(actual);
        }
    }
}