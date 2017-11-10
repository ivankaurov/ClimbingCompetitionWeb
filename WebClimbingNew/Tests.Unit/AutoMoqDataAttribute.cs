using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Climbing.Web.Common.Service;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Common.Service.Repository;
using Climbing.Web.Database;
using Climbing.Web.Model.Facade;
using Climbing.Web.Tests.Unit.Utilities;
using Climbing.Web.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Moq;

namespace Climbing.Web.Tests.Unit
{
    internal sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        private static readonly string DatabaseName = $"Database_{Guid.NewGuid()}";
        public AutoMoqDataAttribute() : base(CreateFixture)
        {
        }

        private static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Register<ClimbingContext>(() =>
            {
                var ctx = new ClimbingContext(
                new DbContextOptionsBuilder<ClimbingContext>().UseInMemoryDatabase(DatabaseName).Options);
                new ContextSeedingHelper(ctx).Seed();
                return ctx;
            });
            
            fixture.Register<IUnitOfWork>(() => fixture.Create<ClimbingContext>());
            fixture.Register<IContextHelper>(() => new SimpleContextHelper());
            fixture.Register<ISeedingHelper>(() => new SimpleSeedingHelper());
            fixture.Register<IPageParameters>(() => fixture.Create<PageParameters>());

            RegisterPagedCollection<TeamFacade>(fixture);

            return fixture;
        }

        private static Fixture RegisterPagedCollection<T>(Fixture fixture)
        {
            fixture.Register<PagedCollection<T>>(() => {
                var collection = fixture.Create<ICollection<T>>();
                return new PagedCollection<T>(collection, fixture.Create<int>(), collection.Count);
            });

            fixture.Register<IPagedCollection<T>>(() => fixture.Create<PagedCollection<T>>());
            return fixture;
        }
    }
}
