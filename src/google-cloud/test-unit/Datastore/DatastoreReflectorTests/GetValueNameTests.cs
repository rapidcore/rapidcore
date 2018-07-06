using System;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace unittests.Datastore.DatastoreReflectorTests
{
    public class GetValueNameTests
    {
        private readonly DatastoreReflector reflector;

        public GetValueNameTests()
        {
            reflector = new DatastoreReflector();
        }
        
        [Fact]
        public void NullPropertiesNotAllowed()
        {
            var actual = Record.Exception(() => reflector.GetValueName(null));

            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot get value name from null{Environment.NewLine}Parameter name: prop", actual.Message);
        }

        [Fact]
        public void UseNameOfProperty()
        {
            var actual = reflector.GetValueName(typeof(DasPoco).GetProperty(nameof(DasPoco.StraightUpNaming)));
            
            Assert.Equal("StraightUpNaming", actual);
        }
        
        [Fact]
        public void UseNameFromAttribute()
        {
            var actual = reflector.GetValueName(typeof(DasPoco).GetProperty(nameof(DasPoco.Aliased)));
            
            Assert.Equal("Nelly", actual);
        }

        #region POCOs
        public class DasPoco
        {
            public string StraightUpNaming { get; set; }
            
            [Name("Nelly")]
            public string Aliased { get; set; }
        }
        #endregion
    }
}