using System;
using RapidCore.FunctionalTests.Migration.Implementation;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.TheMigrations
{
    public class Migration01 : FuncMigrationBase
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