using System;
using RapidCore.Migration;

namespace RapidCore.Redis.FunctionalTest.Migration.TestMigration
{
    public class Migration01 : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            builder.Step("Throw!!", () => throw new InvalidOperationException("I should not be picked up and run"));
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}