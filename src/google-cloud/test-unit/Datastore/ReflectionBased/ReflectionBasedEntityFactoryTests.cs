using System;
using System.Collections.Generic;
using FakeItEasy;
using Google.Cloud.Datastore.V1;
using RapidCore;
using RapidCore.GoogleCloud.Datastore;
using RapidCore.GoogleCloud.Datastore.ReflectionBased;
using Xunit;

namespace unittests.Datastore.ReflectionBased
{
    public class ReflectionBasedEntityFactoryTests
    {
        [Fact]
        public void FromPoco_protectsAgainstInfiniteRecursion()
        {
            // has to be real
            var factory = new ReflectionBasedEntityFactory(new DatastoreReflector());
            var datastoreDb = A.Fake<DatastoreDb>();
            
            var poco = new DasPoco();
            
            // infinite recursion... ouch
            var a = new Infinite();
            var b = new Infinite();
            a.Other = b;
            b.Other = a;
            poco.Infinite = a;
            
            // deep nesting, which is ok
            // it is here to show, that we are "resetting" for every top-level property
            var n1 = new Infinite();
            var n2 = new Infinite {Other = n1};
            var n3 = new Infinite {Other = n2};
            var n4 = new Infinite {Other = n3};
            var n5 = new Infinite {Other = n4};
            var n6 = new Infinite {Other = n5};
            var n7 = new Infinite {Other = n6};
            var n8 = new Infinite {Other = n7};
            var n9 = new Infinite {Other = n8};
            var n10 = new Infinite {Other = n9};
            poco.Nesting = n10;

            var actual = Record.Exception(() => factory.FromPoco(datastoreDb, "kind", poco));

            Assert.IsType<RecursionException>(actual);
            Assert.Equal($"Recursion depth has reached 20 - bailing out.{Environment.NewLine}Path: Infinite.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other.Other", actual.Message);
        }

        #region POCOs
        public class DasPoco
        {
            public string Id => "id";
            public string String => "string";
            public List<string> ListString { get; set; } = new List<string> { "one", "two", "three" };
            public Infinite Infinite { get; set; }
            public Infinite Nesting { get; set; }
        }
        
        public class Infinite
        {
            public Infinite Other { get; set; }
        }
        #endregion
    }
}