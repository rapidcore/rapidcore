using System;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace unittests.Datestore.DatastoreReflectorTests
{
    public class SetIdValueTests
    {
        private readonly DatastoreReflector reflector;
        private readonly KeyFactory keyFactory;

        public SetIdValueTests()
        {
            keyFactory = new KeyFactory("rapidcore-local", "djnamespace", "sokind");
            reflector = new DatastoreReflector();
        }
        
        [Fact]
        public void NullPocosNotAllowed()
        {
            var actual = Record.Exception(() => reflector.SetIdValue(null, keyFactory.CreateKey("yay")));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot set ID on null{Environment.NewLine}Parameter name: poco", actual.Message);
        }

        [Fact]
        public void TypeCheck_short_works()
        {
            var poco = new TypeCheck_Short();
            
            reflector.SetIdValue(poco, keyFactory.CreateKey(5));

            Assert.Equal(5, poco.Id);
        }
        
        [Fact]
        public void TypeCheck_int_works()
        {
            var poco = new TypeCheck_Int();
            
            reflector.SetIdValue(poco, keyFactory.CreateKey(5));

            Assert.Equal(5, poco.Id);
        }
        
        [Fact]
        public void TypeCheck_long_works()
        {
            var poco = new TypeCheck_Long();
            
            reflector.SetIdValue(poco, keyFactory.CreateKey(5));

            Assert.Equal(5, poco.Id);
        }
        
        [Fact]
        public void TypeCheck_string_works()
        {
            var poco = new TypeCheck_String();
            
            reflector.SetIdValue(poco, keyFactory.CreateKey("five"));

            Assert.Equal("five", poco.Id);
        }
        
        [Fact]
        public void TypeCheck_guid_works()
        {
            var poco = new TypeCheck_Guid();
            var guid = Guid.NewGuid();
            
            reflector.SetIdValue(poco, keyFactory.CreateKey(guid.ToString()));

            Assert.Equal(guid, poco.Id);
        }

        #region POCOs
        public class TypeCheck_Short
        {
            public short Id { get; set; } = Int16.MaxValue;
        }
        
        public class TypeCheck_Int
        {
            public int Id { get; set; } = Int32.MaxValue;
        }
        
        public class TypeCheck_Long
        {
            public long Id { get; set; } = Int64.MaxValue;
        }
        
        public class TypeCheck_String
        {
            public string Id { get; set; } = "das id";
        }
        
        public class TypeCheck_Guid
        {
            public Guid Id { get; set; }
        }
        #endregion
    }
}