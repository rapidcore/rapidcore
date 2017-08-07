using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;
using ServiceStack;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Migration.MigrationManagerTests
{
    public class FindMigrationsForUpgradeAsyncTests
    {
        private readonly MigrationManager manager;
        private readonly IConnectionProvider connectionProvider;
        private readonly IContainerAdapter container;
        private readonly MigrationContext context;
        private readonly MongoDbConnection db;

        public FindMigrationsForUpgradeAsyncTests()
        {
            connectionProvider = A.Fake<IConnectionProvider>();
            container = A.Fake<IContainerAdapter>();
            db = A.Fake<MongoDbConnection>();

            context = new MigrationContext
            {
                ConnectionProvider = connectionProvider,
                Container = container,
                Environment = A.Fake<IMigrationEnvironment>(),
                Logger = A.Fake<ILogger>()
            };

            A.CallTo(() => connectionProvider.Default()).Returns(db);
            A.CallTo(() => container.Resolve(A<Type>._)).Returns(null);

            manager = new MigrationManager(new List<Assembly>{typeof(FindMigrationsForUpgradeAsyncTests).GetAssembly()});
        }

        [Fact]
        public async void Returns_unmigrated()
        {
            A.CallTo(() =>
                    db.FirstOrDefaultAsync(MigrationDocument.CollectionName,
                        A<Expression<Func<MigrationDocument, bool>>>._))
                .Returns(Task.FromResult(new MigrationDocument())).NumberOfTimes(2)
                .Then.Returns(Task.FromResult<MigrationDocument>(null));
                
            var actual = await manager.FindMigrationsForUpgradeAsync(context);
            
            Assert.Equal(1, actual.Count);
            Assert.IsType<UnitTestMigration03>(actual.First());
        }
        
        [Fact]
        public async void Returns_orderedList()
        {
            A.CallTo(() => db.FirstOrDefaultAsync(MigrationDocument.CollectionName, A<Expression<Func<MigrationDocument, bool>>>._))
                .Returns(Task.FromResult<MigrationDocument>(null));
                
            var actual = await manager.FindMigrationsForUpgradeAsync(context);
            
            Assert.Equal(3, actual.Count);
            Assert.IsType<UnitTestMigration01>(actual[0]);
            Assert.IsType<UnitTestMigration02>(actual[1]);
            Assert.IsType<UnitTestMigration03>(actual[2]);
        }
    }

    #region Migrations
    public class UnitTestMigration01 : IMigration
    {
        public Task UpgradeAsync(MigrationContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task DowngradeAsync(MigrationContext context)
        {
            throw new System.NotImplementedException();
        }

        public string Name => GetType().Name;
    }
    
    public class UnitTestMigration02 : IMigration
    {
        public Task UpgradeAsync(MigrationContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task DowngradeAsync(MigrationContext context)
        {
            throw new System.NotImplementedException();
        }

        public string Name => GetType().Name;
    }
    
    public class UnitTestMigration03 : IMigration
    {
        public Task UpgradeAsync(MigrationContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task DowngradeAsync(MigrationContext context)
        {
            throw new System.NotImplementedException();
        }

        public string Name => GetType().Name;
    }
    #endregion
}