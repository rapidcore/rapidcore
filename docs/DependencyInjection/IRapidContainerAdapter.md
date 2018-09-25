# IRapidContainerAdapter

The `RapidCore.DependencyInjection.IRapidContainerAdapter` interface, is what we use in parts of RapidCore that need a container/factory to create or provide instances of classes.

It is quite simple to implement, as it only contains 3 resolve methods, that should map to methods provided by most Container implementations.

## Using it with IServiceProvider

As it was easy to do and most likely useful for a lot of people, we have included an implementation of the adapter for use with `System.IServiceProvider` in the form of `RapidCore.DependencyInjection.ServiceProviderRapidContainerAdapter`.
