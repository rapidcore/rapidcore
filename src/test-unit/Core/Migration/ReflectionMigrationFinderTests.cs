using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using RapidCore.DependencyInjection;
using RapidCore.Migration;
using Xunit;

namespace UnitTests.Core.Migration
{
    public class ReflectionMigrationFinderTests
    {
        private readonly ReflectionMigrationFinder finder;
        private readonly IMigrationContext context;
        private readonly IRapidContainerAdapter container;
        private readonly IMigrationStorage storage;

        public ReflectionMigrationFinderTests()
        {
            context = A.Fake<IMigrationContext>();
            container = A.Fake<IRapidContainerAdapter>();
            storage = A.Fake<IMigrationStorage>();

            A.CallTo(() => context.Container).Returns(container);
            A.CallTo(() => context.Storage).Returns(storage);
            
            A.CallTo(() => container.Resolve(A<Type>._)).Returns(null);
            
            finder = new ReflectionMigrationFinder(typeof(ReflectionMigrationFinderTests).GetTypeInfo().Assembly);
        }
        
        [Fact]
        public async void Returns_unmigrated()
        {
            // first 2 migrations have been completed.. the 3rd has not
            A.CallTo(() => storage.HasMigrationBeenFullyCompletedAsync(context, A<string>._))
                .Returns(true).NumberOfTimes(2)
                .Then.Returns(false);
                
            var actual = await finder.FindMigrationsForUpgradeAsync(context);
            
            Assert.Equal(1, actual.Count);
            Assert.IsType<UnitTestMigration03>(actual.First());
        }
        
        [Fact]
        public async void Returns_orderedList()
        {
            // none of the migrations have been completed
            A.CallTo(() => storage.HasMigrationBeenFullyCompletedAsync(context, A<string>._)).Returns(false);
                
            var actual = await finder.FindMigrationsForUpgradeAsync(context);
            
            Assert.Equal(3, actual.Count);
            Assert.IsType<UnitTestMigration01>(actual[0]);
            Assert.IsType<UnitTestMigration02>(actual[1]);
            Assert.IsType<UnitTestMigration03>(actual[2]);
        }
    }
    
    #region Migrations
    public class UnitTestMigration01 : NotImplementedMigrationBase {}
   
    public class UnitTestMigration02 : NotImplementedMigrationBase {}
    
    public class UnitTestMigration03 : NotImplementedMigrationBase {}
    #endregion
}