namespace Climbing.Web.Tests.Unit.Utilities
{
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service;

    internal sealed class SimpleContextHelper : IContextHelper
    {
        public Task<bool> IsMigrated(CancellationToken cancellationToken) => Task.FromResult(true);

        public Task Migrate(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}