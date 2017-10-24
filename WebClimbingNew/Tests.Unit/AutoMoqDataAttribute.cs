using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Services;
using Database;
using Tests.Unit.Utilities;

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
            fixture.Register<IIdentityProvider>(() => IdentityProvider.Instance);
            return fixture;
        }
    }
}
