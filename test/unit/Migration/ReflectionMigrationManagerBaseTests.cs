using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.DependencyInjection;
using RapidCore.Migration;
using Xunit;

namespace RapidCore.UnitTests.Migration
{
    public class ReflectionMigrationManagerBaseTests
    {
        private readonly TestReflectionMigrationManagerBase manager;
        private readonly IMigrationContext context;
        private readonly IRapidContainerAdapter container;

        public ReflectionMigrationManagerBaseTests()
        {
            context = A.Fake<IMigrationContext>();
            container = A.Fake<IRapidContainerAdapter>();

            A.CallTo(() => context.Container).Returns(container);
            A.CallTo(() => container.Resolve(A<Type>._)).Returns(null);
            
            manager = new TestReflectionMigrationManagerBase(typeof(ReflectionMigrationManagerBaseTests).GetTypeInfo().Assembly);
        }
        
        [Fact]
        public async void Returns_unmigrated()
        {
            manager.Checker = A.Fake<Func<bool>>();
            
            // first 2 migrations have been completed.. the 3rd has not
            A.CallTo(() => manager.Checker.Invoke()).Returns(true).NumberOfTimes(2).Then.Returns(false);
                
            var actual = await manager.FindMigrationsForUpgradeAsync(context);
            
            Assert.Equal(1, actual.Count);
            Assert.IsType<UnitTestMigration03>(actual.First());
        }
        
        [Fact]
        public async void Returns_orderedList()
        {
            manager.Checker = A.Fake<Func<bool>>();
            
            // none of the migrations have been completed
            A.CallTo(() => manager.Checker.Invoke()).Returns(false);
                
            var actual = await manager.FindMigrationsForUpgradeAsync(context);
            
            Assert.Equal(3, actual.Count);
            Assert.IsType<UnitTestMigration01>(actual[0]);
            Assert.IsType<UnitTestMigration02>(actual[1]);
            Assert.IsType<UnitTestMigration03>(actual[2]);
        }
        

        #region Implementation
        private class TestReflectionMigrationManagerBase : ReflectionMigrationManagerBase
        {
            public TestReflectionMigrationManagerBase(IList<Assembly> assemblies) : base(assemblies)
            {
            }

            public TestReflectionMigrationManagerBase(Assembly assembly) : base(assembly)
            {
            }

            public override async Task MarkAsCompleteAsync(IMigration migration, long milliseconds, IMigrationContext context)
            {
                await Task.CompletedTask;
            }

            protected override Task<bool> HasMigrationBeenFullyCompletedAsync(string migrationName, IMigrationContext context)
            {
                return Task.FromResult(Checker());
            }
            
            public Func<bool> Checker { get; set; }
        }
        #endregion
    }
    
    #region Migrations
    public class UnitTestMigration01 : NotImplementedMigrationBase {}
   
    public class UnitTestMigration02 : NotImplementedMigrationBase {}
    
    public class UnitTestMigration03 : NotImplementedMigrationBase {}
    #endregion
}