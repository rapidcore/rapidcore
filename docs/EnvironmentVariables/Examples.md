# Examples

## Get the value of an environment variable

```csharp
using RapidCore.Environment;

public class SomeThingKewl
{
    private readonly EnvironmentVariables envVars;

    public SomeThingKewl(EnvironmentVariables envVars)
    {
        this.envVars = envVars;
    }

    public void AmazingAction()
    {
        /**
         * Get the value of the REMOTE_URL environment variable.
         * If it does not exist or is empty, then "http://example.com/" is returned.
         */
        string remoteUrl = envVars.Get("REMOTE_URL", "http://example.com/");

        int maxAttempts = envVars.Get("MAX_ATTEMPTS", 5);

        bool doX = envVars.Get("DO_X", false);
    }
}
```
