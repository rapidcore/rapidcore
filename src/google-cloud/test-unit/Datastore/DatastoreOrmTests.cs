using System;
using System.Collections.Generic;
using FakeItEasy;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace unittests.Datastore
{
    public class DatastoreOrmTests
    {
        private readonly DatastoreOrm orm;
        
        private readonly DatastoreDb datastoreDb;
        private readonly DatastoreReflector reflector;
        private readonly IEntityFactory entityFactory;
        private readonly IPocoFactory pocoFactory;
        private readonly Entity entity;

        public DatastoreOrmTests()
        {
            reflector = A.Fake<DatastoreReflector>();
            entityFactory = A.Fake<IEntityFactory>();
            datastoreDb = A.Fake<DatastoreDb>();
            pocoFactory = A.Fake<IPocoFactory>();
            entity = new Entity();

            A.CallTo(() => entityFactory.FromPoco(datastoreDb, A<string>._, A<object>._)).Returns(entity);
            A.CallTo(() => reflector.GetKind(A<Type>._)).Returns("SoKind");
            
            orm = new DatastoreOrm(
                datastoreDb,
                reflector,
                entityFactory,
                pocoFactory
            );
        }

        [Fact]
        public void PocoToEntity_poco_throwsIfGivenNull()
        {
            var actual = Record.Exception(() => orm.PocoToEntity(null));
            
            Assert.IsType<ArgumentNullException>(actual);
            Assert.Equal($"Cannot create an Entity from null{Environment.NewLine}Parameter name: poco", actual.Message);
        }
        
        [Fact]
        public void PocoToEntity_poco_infersKind()
        {
            var poco = new Bee();
            
            var actual = orm.PocoToEntity(poco);

            A.CallTo(() => reflector.GetKind(typeof(Bee))).MustHaveHappenedOnceExactly();
            A.CallTo(() => entityFactory.FromPoco(datastoreDb, "SoKind", poco)).MustHaveHappenedOnceExactly();
            
            Assert.Same(entity, actual);
        }

        [Fact]
        public void GetValueName_works_given_a_property()
        {
            var actual = orm.GetValueName<ValueNameVictim>(x => x.MyProperty);
            
            Assert.Equal("MyProperty", actual);
        }
        
        [Fact]
        public void GetValueName_throws_ifExpression_IsWrongType()
        {
            var actual = Record.Exception(() => orm.GetValueName<ValueNameVictim>(x => x.MyProperty.Equals("horse")));

            Assert.IsType<ArgumentException>(actual);
            Assert.StartsWith("The given expression does not point to a member", actual.Message);
        }
        
        [Fact]
        public void GetValueName_throws_ifExpression_doesNotPointToAProperty()
        {
            var actual = Record.Exception(() => orm.GetValueName<ValueNameVictim>(x => x.SomeField));

            Assert.IsType<ArgumentException>(actual);
            Assert.StartsWith("The given expression does not point to a property", actual.Message);
        }
        
        [Fact]
        public void GetValueName_1_level_only()
        {
            var actual = Record.Exception(() => orm.GetValueName<ValueNameVictim>(x => x.TheDeeper.DangerIs));

            Assert.IsType<ArgumentException>(actual);
            Assert.StartsWith("The given expression does not point to a member", actual.Message);
        }

        [Fact]
        public void GetValueName_works_with_DateTime()
        {
            var actual = orm.GetValueName<ValueNameVictim>(x => x.MyDateTime);
            
            Assert.Equal("MyDateTime", actual);
        }
        
        [Fact]
        public void GetValueName_works_with_Enum()
        {
            var actual = orm.GetValueName<ValueNameVictim>(x => x.MyEnum);
            
            Assert.Equal("MyEnum", actual);
        }
        
        [Fact]
        public void GetValueName_works_with_List()
        {
            var actual = orm.GetValueName<ValueNameVictim>(x => x.MyList);
            
            Assert.Equal("MyList", actual);
        }
        
        [Fact]
        public void GetValueName_works_with_int()
        {
            var actual = orm.GetValueName<ValueNameVictim>(x => x.MyInt);
            
            Assert.Equal("MyInt", actual);
        }

        #region POCOs
        public class Bee {}
        
        public class ValueNameVictim
        {
            public string SomeField;
            
            public string MyProperty { get; set; }
            
            public Deeper TheDeeper { get; set; }
            
            public DateTime MyDateTime { get; set; }
            
            public Greeting MyEnum { get; set; }
            
            public List<string> MyList { get; set; }
            
            public int MyInt { get; set; }
        }
        
        public class Deeper
        {
            public int DangerIs => 23;
        }
        
        public enum Greeting
        {
            Hello = 0,
            Hi
        }
        #endregion
    }
}