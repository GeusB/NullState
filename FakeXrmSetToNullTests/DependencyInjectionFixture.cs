using System;
using FakeXrmSetToNull;
using Microsoft.Extensions.DependencyInjection;

namespace FakeXrmSetToNullTests
{
    public class DependencyInjectionFixture : IDisposable
    {
        public DependencyInjectionFixture()
        {
            DependencyInjection.Register(services =>
            {
                services.AddScoped<IXrmContext, XrmTestsContext>();
                services.AddTransient<ILogic, Logic>();
            });
        }
        public void Dispose()
        {

        }
    }
}
