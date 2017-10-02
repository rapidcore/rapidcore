using System;
using System.Threading.Tasks;
using RapidCore.Migration;

namespace RapidCore.UnitTests.Migration
{
    /// <summary>
    /// An <see cref="IMigration"/> that throws <see cref="NotImplementedException"/>,
    /// but implements <see cref="Name"/>
    /// </summary>
    public abstract class NotImplementedMigrationBase : IMigration
    {
        public Task UpgradeAsync(IMigrationContext context)
        {
            throw new NotImplementedException();
        }

        public Task DowngradeAsync(IMigrationContext context)
        {
            throw new NotImplementedException();
        }

        public string Name => GetType().Name;
    }
}