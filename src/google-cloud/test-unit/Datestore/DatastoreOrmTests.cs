using System;
using FakeItEasy;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore;
using Xunit;

namespace unittests.Datestore
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

        #region POCOs
        public class Bee {}
        #endregion
    }
}