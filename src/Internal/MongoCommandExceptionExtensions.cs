using System;
using MongoDB.Driver;

namespace RapidCore.Mongo.Internal
{
    public static class MongoCommandExceptionExtensions
    {
        public static bool EnsureIndexesShouldDropAndRetry(this MongoCommandException ex, IndexDefinition index)
        {
            //
            // index already exists, but with different options
            //
            if (ex.Message.Equals($"Command createIndexes failed: Index with name: {index.Name} already exists with different options."))
            {
                return true;
            }

            //
            // index already exists
            //
            else if (ex.Message.StartsWith("Command createIndexes failed: Index must have unique name.The existing index:"))
            {
                // check if the existing and the requested index are different

                var indexes = ex.Message
                    .Remove(0, "Command createIndexes failed: Index must have unique name.The existing index: ".Length)
                    .Split(new string[] { " has the same name as the requested index: " }, StringSplitOptions.None);

                return !indexes[0].Trim().Equals(indexes[1].Trim());
            }

            return false;
        }
    }
}