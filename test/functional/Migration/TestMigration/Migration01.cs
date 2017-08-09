using System;
using RapidCore.Mongo.Migration;

namespace RapidCore.Mongo.FunctionalTests.Migration.TestMigration
{
    public class Migration01 : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            builder.Step("Throw!!", () => throw new InvalidOperationException("I should not be picked up and run"));
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}