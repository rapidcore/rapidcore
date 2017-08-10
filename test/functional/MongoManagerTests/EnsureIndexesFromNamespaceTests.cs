using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using RapidCore.Mongo.FunctionalTests.MongoManagerTests.Sub;
using RapidCore.Mongo.Internal;
using ServiceStack;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests.MongoManagerTests
{
    public class EnsureIndexesFromNamespaceTests : MongoManagerTestsBase
    {
        [Fact]
        public void IndexesOnlyTheStuffFromTheNamespace()
        {
            var collectionName = typeof(EnsureIndexTestInNamespace).GetTypeInfo().GetCollectionName();
            EnsureEmptyCollection(collectionName);
            GetClient().DropDatabase(GetDbName());
            
            manager.EnsureIndexes(GetDb(), typeof(EnsureIndexTestInNamespace).GetAssembly(), typeof(EnsureIndexTestInNamespace).Namespace);

            var actual = GetIndexes<EnsureIndexTestInNamespace>(collectionName);

            //
            // the index was created
            //
            Assert.Equal(2, actual.Count); // the auto-generated "_id_" and "hephey"
            Assert.Equal(new BsonDocument().Add("Hephey", 1), actual["hephey"].GetElement("key").Value);
            
            //
            // did we create anything else?
            //
            var collectionList = GetDb().ListCollections().ToList();
            Assert.Equal(1, collectionList.Count);
            Assert.Equal(collectionName, collectionList.First().GetElement("name").Value);
        }
    }
}

#region entity
namespace RapidCore.Mongo.FunctionalTests.MongoManagerTests.Sub
{
    [Entity]
    public class EnsureIndexTestInNamespace
    {
        [Index(Name = "hephey")]
        public string Hephey { get; set; }
    }
}
#endregion