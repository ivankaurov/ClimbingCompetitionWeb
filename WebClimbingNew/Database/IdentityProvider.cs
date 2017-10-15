using Database.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    internal sealed class IdentityProvider : IIdentityProvider<Guid>
    {
        private IdentityProvider()
        {
        }

        public static IIdentityProvider<Guid> Instance { get; } = new IdentityProvider();

        public Guid CreateNewIdentity() => Guid.NewGuid();
    }
}
