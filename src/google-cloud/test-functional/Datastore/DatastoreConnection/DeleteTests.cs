using Google.Cloud.Datastore.V1;
using Xunit;

namespace functionaltests.Datastore.DatastoreConnection
{
    public class DeleteTests : DatastoreConnectionTestBase
    {
        [Fact]
        public async void Delete_long()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                EnsureEmptyKind("TheDeleter");

                Insert(new Entity
                {
                    Key = GetKey("TheDeleter", 5),
                    ["String"] = "hello"
                });

                await connection.DeleteAsync<TheDeleter>(5);

                var all = GetAll("TheDeleter");
                Assert.Empty(all);
            });
        }
        
        [Fact]
        public async void Delete_string()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                EnsureEmptyKind("TheDeleter");

                Insert(new Entity
                {
                    Key = GetKey("TheDeleter", "5"),
                    ["String"] = "hello"
                });

                await connection.DeleteAsync<TheDeleter>("5");

                var all = GetAll("TheDeleter");
                Assert.Empty(all);
            });
        }
        
        [Fact]
        public async void Delete_long_kind()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                EnsureEmptyKind("TheDeleter");

                Insert(new Entity
                {
                    Key = GetKey("TheDeleter", 5),
                    ["String"] = "hello"
                });

                await connection.DeleteAsync(5, "TheDeleter");

                var all = GetAll("TheDeleter");
                Assert.Empty(all);
            });
        }
        
        [Fact]
        public async void Delete_string_kind()
        {
            await WorkAroundDatastoreEmulatorIssueAsync(async () =>
            {
                EnsureEmptyKind("TheDeleter");

                Insert(new Entity
                {
                    Key = GetKey("TheDeleter", "5"),
                    ["String"] = "hello"
                });

                await connection.DeleteAsync("5", "TheDeleter");

                var all = GetAll("TheDeleter");
                Assert.Empty(all);
            });
        }

        #region POCOs
        public class TheDeleter
        {
        }
        #endregion
    }
}