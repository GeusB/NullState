using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace FakeXrmSetToNull
{
    public static class DependencyInjection
    {
        public static IServiceProvider ServiceProvider;

        public static readonly ConcurrentDictionary<Guid, IServiceScope> Scopes =
            new ConcurrentDictionary<Guid, IServiceScope>();

        public static void Register(Action<ServiceCollection> registerServices)
        {
            var services = new ServiceCollection();
            registerServices(services);
            ServiceProvider = services.BuildServiceProvider(true);
        }

        public static T GetService<T>(Guid? functionInstanceId = null) where T : class
        {
            if (functionInstanceId != null && Scopes.TryGetValue(functionInstanceId.Value, out var scope))
                return scope.ServiceProvider.GetRequiredService<T>();

            return ServiceProvider.GetRequiredService<T>();
        }

        public static IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }
    }
}