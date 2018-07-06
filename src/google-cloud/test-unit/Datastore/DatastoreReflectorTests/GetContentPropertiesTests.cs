using System;
using RapidCore;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace unittests.Datastore.DatastoreReflectorTests
{
    public class GetContentPropertiesTests
    {
        private readonly DatastoreReflector reflector;

        public GetContentPropertiesTests()
        {
            reflector = new DatastoreReflector();
        }
        
        [Fact]
        public void NullPocosNotAllowed()
        {
            var actual = Record.Exception(() => reflector.GetContentProperties(null));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot get content properties from null{Environment.NewLine}Parameter name: poco", actual.Message);
        }

        [Fact]
        public void ReturnsEmptyList_ifNoProperties()
        {
            var actual = reflector.GetContentProperties(new OnlyHasFields());
            
            Assert.Empty(actual);
        }
        
        [Fact]
        public void Respects_Ignore()
        {
            var actual = reflector.GetContentProperties(new OneIgnoredProperty());
            
            Assert.Empty(actual);
        }
        
        [Fact]
        public void WorksWithInheritance()
        {
            var actual = reflector.GetContentProperties(new HasBaseClass());
            
            Assert.Equal(2, actual.Count);
            Assert.Contains(actual, x => x.Name == "OnBase");
            Assert.Contains(actual, x => x.Name == "OnChild");
        }
        
        [Fact]
        public void Supporst_ReadOnly_properties()
        {
            var actual = reflector.GetContentProperties(new WithReadOnly());
            
            Assert.Equal(1, actual.Count);
            Assert.Contains(actual, x => x.Name == "ReadOnly");
        }
        
        [Fact]
        public void Ignores_Private_properties()
        {
            var actual = reflector.GetContentProperties(new PrivateOnly());
            
            Assert.Empty(actual);
        }
        
        [Fact]
        public void Ignores_static_properties()
        {
            var actual = reflector.GetContentProperties(new StaticOnly());
            
            Assert.Empty(actual);
        }
        
        [Fact]
        public void Ignores_properties_without_getters()
        {
            var actual = reflector.GetContentProperties(new NoGetter());
            
            Assert.Empty(actual);
        }
        
        [Fact]
        public void Ignores_id_properties_byName()
        {
            var actual = reflector.GetContentProperties(new HasIdByName());
            
            Assert.Empty(actual);
        }
        
        [Fact]
        public void Ignores_id_properties_byAttribute()
        {
            var actual = reflector.GetContentProperties(new HasIdByAttribute());
            
            Assert.Empty(actual);
        }

        #region POCOs
        public class OnlyHasFields
        {
            private string field;
        }
        
        public class OneIgnoredProperty
        {
            [Ignore]
            public string One { get; set; }
        }
        
        public abstract class BaseClass
        {
            public string OnBase { get; set; }
        }
        
        public class HasBaseClass : BaseClass
        {
            public string OnChild { get; set; }
        }
        
        public class WithReadOnly
        {
            public string ReadOnly => "read only";
        }
        
        public class PrivateOnly
        {
            private string One { get; set; }
        }
        
        public class StaticOnly
        {
            public static string One { get; set; }
        }
        
        public class NoGetter
        {
            private string field;
            public string Field
            {
                set => field = value;
            }
        }
        
        public class HasIdByName
        {
            public string Id { get; set; }
        }
        
        public class HasIdByAttribute
        {
            [PrimaryKey]
            public string One { get; set; }
        }
        #endregion
    }
}