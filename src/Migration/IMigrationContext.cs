using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;

namespace RapidCore.Migration
{
    public interface IMigrationContext
    {
        ILogger Logger { get; set; }
        
        IRapidContainerAdapter Container { get; set; }
        
        IMigrationEnvironment Environment { get; set; }
    }
}