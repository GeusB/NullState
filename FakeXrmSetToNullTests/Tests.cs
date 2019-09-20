using System.Collections.Generic;
using System.Linq;
using FakeXrmEasy;
using FakeXrmSetToNull;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using Xrm.CertX.Shared.Entities;
using Xunit;
using IXrmContext = FakeXrmSetToNull.IXrmContext;

namespace FakeXrmSetToNullTests
{
    public class Tests : IClassFixture<DependencyInjectionFixture>
    {
        private static XrmFakedContext GetXrmFakedContext(List<Entity> entities = null, IServiceScope scope = null)
        {
            var context = new XrmFakedContext();

            if (entities == null)
                entities = new List<Entity>();

            context.Initialize(entities);

            var xrmService = context.GetOrganizationService();

            XrmTestsContext xrmContext;
            if (scope != null)
                xrmContext = scope.ServiceProvider.GetService<IXrmContext>() as XrmTestsContext;
            else
                xrmContext = DependencyInjection.GetService<IXrmContext>() as XrmTestsContext;

            xrmContext.XrmService = xrmService;

            return context;
        }

        [Fact]
        public void Test1()
        {
            using (var scope = DependencyInjection.CreateScope())
            {
                var context = GetXrmFakedContext(scope: scope);
                var calculationsService = scope.ServiceProvider.GetService<ILogic>();


                calculationsService.Process();


                var bonusQuery = context.CreateQuery<cgk_bonus>();
                bonusQuery.Count().Should().Be(3);
            }
        }
    }
}