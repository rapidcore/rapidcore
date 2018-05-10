using System;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Datastore.Internal;
using Xunit;

namespace unittests.Datestore.Internal.DatastoreReflectorTests
{
    public class GetKindTests
    {
        private readonly DatastoreReflector reflector;

        public GetKindTests()
        {
            reflector = new DatastoreReflector();
        }
        
        [Fact]
        public void NullPocosNotAllowed()
        {
            var actual = Record.Exception(() => reflector.GetKind((object)null));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot get kind from null{Environment.NewLine}Parameter name: poco", actual.Message);
        }

        [Fact]
        public void BasedOnPocoName()
        {
            var actual = reflector.GetKind(new PocoNoKindAttr());
            
            Assert.Equal("PocoNoKindAttr", actual);
        }
        
        [Fact]
        public void BasedOnKindAttribute()
        {
            var actual = reflector.GetKind(new PocoWithKindAttr());
            
            Assert.Equal("nice", actual);
        }

        #region POCOs
        public class PocoNoKindAttr
        {
            public string String { get; set; }
        }
        
        [Kind("nice")]
        public class PocoWithKindAttr
        {
            public string String { get; set; }
        }
        #endregion
    }
}