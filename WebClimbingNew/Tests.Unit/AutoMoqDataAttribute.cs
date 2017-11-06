using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Climbing.Web.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

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

            return fixture;
        }
    }
}
