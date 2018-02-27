using System;
using System.Collections.Generic;
using System.Text;

namespace RapidCore.PostgreSql.Internal
{
    public static class PostgreSqlConstants
    {
        public static string MigrationInfoTableName { get; } = "__RapidCoreMigrations";
        public static string CompletedStepsTableName { get; } = "__RapidCoreMigrationsCompletedSteps";
    }
}
