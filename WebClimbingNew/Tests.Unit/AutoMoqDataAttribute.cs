using System;
using System.Threading;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Climbing.Web.Common.Service;
using Climbing.Web.Common.Service.Repository;
using Climbing.Web.Database;
using Climbing.Web.Tests.Unit.Utilities;
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
            fixture.Register<ClimbingContext>(() => new ClimbingContext(
                new DbContextOptionsBuilder<ClimbingContext>().UseInMemoryDatabase(DatabaseName).Options));
            fixture.Register<IUnitOfWork>(() => fixture.Create<ClimbingContext>());
            fixture.Register<IContextHelper>(() => new SimpleContextHelper());
            fixture.Register<ISeedingHelper>(() => new SimpleSeedingHelper());

            return fixture;
        }
    }
}
