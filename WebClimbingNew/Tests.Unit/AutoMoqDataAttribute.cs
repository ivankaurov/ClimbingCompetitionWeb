using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Database;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Tests.Unit
{
    internal sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(CreateFixture)
        {
        }

        private static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Register<ValueGenerator<string>>(() => new IdentityProvider());
            return fixture;
        }
    }
}
