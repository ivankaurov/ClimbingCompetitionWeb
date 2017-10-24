using Database.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    internal sealed class IdentityProvider : IIdentityProvider
    {
        private IdentityProvider()
        {
        }

        public static IIdentityProvider Instance { get; } = new IdentityProvider();

        public Guid CreateNewIdentity() => Guid.NewGuid();
    }
}
