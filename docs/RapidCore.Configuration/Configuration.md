# Reading configuration values

It is extremely rare to deal with an application that does not need to read configuration values.

The common pattern these days - in a world of containerized applications - is to have some default configuration in files and allowing specific config values to be overruled using environment variables.


## [Obsolete] `ConfigBase`

The abstract `RapidCore.Configuration.ConfigBase` class exists to make it easy to deliver a strict config interface for use in an application.

An example:

```csharp
public interface IShoutConfig
{
    string WhatToShout { get; }
    int HowManyTimesToShout { get; }
    ShoutColor Color { get; }
}

public enum ShoutColor
{
    Red,
    Green,
    Purple
}

public class ShoutConfig : ConfigBase, IShoutConfig
{
    public MyConfig(IConfiguration config) : base(config) {}

    public string WhatToShout => Get("WHAT_TO_SHOUT", "default shout");
    public int HowManyTimesToShout => Get("HOW_MANY_TIMES_TO_SHOUT", 666);
    public ShoutColor Color => Get("SHOUT_COLOR", ShoutColor.Purple);
}
```

With the above, our `Shouter` class just needs to depend on `IShoutConfig` and it will automatically get type appropriate configuration values - including defaults. This allows the code to not have to duplicate the parsing/handling of the raw configuration values all over the place.


## Extension methods on `IConfiguration`

The logic implemented in `ConfigBase` does not actually need to be in a class. Instead the logic has been moved to extension methods on `Microsoft.Extensions.Configuration.IConfiguration`, which means that it is no longer necessary to extend `ConfigBase` (and it has actually been marked as _obsolete_).

### Get<T>(string key, T defaultValue)

This method converts the value hiding behind `key` to `T` or returns `defaultValue` if there was no value to be found.

### Get<T>(IEnumerable<string> keys, T defaultValue)

This method does the same thing as the regular Get, but it attempts to use each key in **in order**. If none of the keys result in a value, then `defaultValue` is returned.

This one also has 2 convenience overloads for either 2 or 3 keys.

The use case that drove this was - specifically - dealing with a service oriented monolith (i.e. 1 executable with multiple services inside that are treated as silos), where each service needed its own database and credentials. To begin with, these systems tend to use 1 database instance, which means that each service would duplicate the host and port of the database instance in their configurations - e.g.

```json
{
    "db_a": { "host": "shared_host", "name": "a" },
    "db_b": { "host": "shared_host", "name": "b" }
}
```

By allowing the config class to check on multiple keys, the following can be done to deduplicate the configuration values.

```json
{
    "db_shared": { "host": "shared_host" },
    "db_a": { "name": "a" },
    "db_b": { "name": "b" }
}
```

In the config classes it is now possible to do `public string Host => myIConfiguration.Get("db_a:host", "db_shared:host", "no host")`.


### GetCommaSeparatedList<T>(string key, List<T> defaultValue)

A common case is to have a configuration value that is actually a list of something. Since it (probably) needs to be represented as a string in an environment variable, a _comma separated string_ is an easy pattern.

For this purpose the `GetCommaSeparatedList<T>(string key, List<T> defaultValue)` method exists - note that it also supports trying multiple keys.

```json
{
    "characters": "joker, mr. freeze, , poison ivy"
}
```

```csharp
IConfiguration config = __magic__;

var characters = config.GetCommaSeparatedList("characters", new List<string>(0));
// 0 = joker
// 1 = mr. freeze
// 2 = poison ivy
```

Notice how it trims and removes empty entries.
