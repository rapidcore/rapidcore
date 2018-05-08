using System;
using RapidCore;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Datastore.Internal;
using Xunit;

namespace unittests.Datestore.Internal.DatastoreReflectorTests
{
    public class GetIdValueTests
    {
        private readonly DatastoreReflector reflector;

        public GetIdValueTests()
        {
            reflector = new DatastoreReflector();
        }

        [Fact]
        public void NullPocosNotAllowed()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(null));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal("Cannot get ID from null\nParameter name: poco", actual.Message);
        }
        
        [Fact]
        public void Property_withAttribute()
        {
            var actual = reflector.GetIdValue(new WithAttribute());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void PropertyNamed_ID()
        {
            var actual = reflector.GetIdValue(new PropNamed_ID());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void PropertyNamed_Id()
        {
            var actual = reflector.GetIdValue(new PropNamed_Id());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void PropertyNamed_id()
        {
            var actual = reflector.GetIdValue(new PropNamed_id());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void PropertyNamed_IdentiFIER()
        {
            var actual = reflector.GetIdValue(new PropNamed_IdentiFIer());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void PropertyNamed_Primary_Key()
        {
            var actual = reflector.GetIdValue(new PropNamed_Primary_Key());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void PropertyNamed_PrimaryKey()
        {
            var actual = reflector.GetIdValue(new PropNamed_PrimaryKey());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void Only_1_primary_key_allowed()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(new MultipleIds()));

            Assert.IsType<PrimaryKeyException>(actual);
            Assert.Equal("More than 1 property on MultipleIds could be an id: One, PrimaryKey", actual.Message);
        }
        
        [Fact]
        public void Throw_if_no_id()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(new NoId()));

            Assert.IsType<PrimaryKeyException>(actual);
            Assert.Equal("Could not find an id on NoId", actual.Message);
        }
        
        [Fact]
        public void WorksWithInheritance()
        {
            var actual = reflector.GetIdValue(new WithBaseClass());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void Must_haveAGetter()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(new NoGetter()));

            Assert.IsType<PrimaryKeyException>(actual);
            Assert.Equal("The id property NoGetter.Id has no getter", actual.Message);
        }
        
        [Fact]
        public void Must_haveANonStaticGetter()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(new Static()));

            Assert.IsType<PrimaryKeyException>(actual);
            Assert.Equal("The id property Static.Id is static", actual.Message);
        }
        
        [Fact]
        public void Must_notBeIgnored()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(new IdHasIgnore()));

            Assert.IsType<PrimaryKeyException>(actual);
            Assert.Equal("The id property IdHasIgnore.Id is marked with IgnoreAttribute", actual.Message);
        }
        
        [Fact]
        public void TypeCheck_short_isValid()
        {
            var actual = reflector.GetIdValue(new TypeCheck_Short());
            
            Assert.Equal(short.MaxValue.ToString(), actual);
        }
        
        [Fact]
        public void TypeCheck_int_isValid()
        {
            var actual = reflector.GetIdValue(new TypeCheck_Int());
            
            Assert.Equal(int.MaxValue.ToString(), actual);
        }
        
        [Fact]
        public void TypeCheck_long_isValid()
        {
            var actual = reflector.GetIdValue(new TypeCheck_Long());
            
            Assert.Equal(long.MaxValue.ToString(), actual);
        }
        
        [Fact]
        public void TypeCheck_string_isValid()
        {
            var actual = reflector.GetIdValue(new TypeCheck_String());
            
            Assert.Equal("das id", actual);
        }
        
        [Fact]
        public void TypeCheck_guid_isValid()
        {
            var expected = Guid.NewGuid();
            
            var actual = reflector.GetIdValue(new TypeCheck_Guid { Id = expected });
            
            Assert.Equal(expected.ToString(), actual);
        }
        
        [Fact]
        public void TypeCheck_double_INVALID()
        {
            var actual = Record.Exception(() => reflector.GetIdValue(new TypeCheck_Invalid_Double()));
            
            Assert.IsType<PrimaryKeyException>(actual);
            Assert.Equal("The id property TypeCheck_Invalid_Double.Id has invalid type of Double. Only Int16, Int32, Int64, String, Guid are allowed.", actual.Message);
        }

        #region POCOs
        public class WithAttribute
        {
            [PrimaryKey]
            public string One { get; set; } = "das id";
        }
        
        public class PropNamed_ID
        {
            public string ID { get; set; } = "das id";
        }
        
        public class PropNamed_Id
        {
            public string Id { get; set; } = "das id";
        }
        
        public class PropNamed_id
        {
            public string id { get; set; } = "das id";
        }
        
        public class PropNamed_IdentiFIer
        {
            public string IdentiFIer { get; set; } = "das id";
        }
        
        public class PropNamed_Primary_Key
        {
            public string Primary_Key { get; set; } = "das id";
        }
        
        public class PropNamed_PrimaryKey
        {
            public string PrimaryKey { get; set; } = "das id";
        }
        
        public class MultipleIds
        {
            [PrimaryKey]
            public string One { get; set; }
            public string PrimaryKey { get; set; }
        }
        
        public class NoId
        {
            public string One { get; set; }
        }
        
        public abstract class HasId
        {
            public string Id { get; set; } = "das id";
        }
        
        public class WithBaseClass : HasId
        {
            public string One { get; set; }
        }
        
        public class NoGetter
        {
            private string id;
            public string Id
            {
                set => id = value;
            }
        }
        
        public class Static
        {
            public static string Id { get; set; } // static
        }
        
        public class IdHasIgnore
        {
            [Ignore]
            public string Id { get; set; }
        }

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
        
        public class TypeCheck_Invalid_Double
        {
            public double Id { get; set; }
        }
        #endregion
    }
}