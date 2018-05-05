namespace Climbing.Web.Tests.Unit.Utilities
{
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service.Repository;

    internal sealed class SimpleSeedingHelper : ISeedingHelper
    {
        public Task<bool> IsSeeded(CancellationToken cancellationToken = default(CancellationToken)) => Task.FromResult(true);

        public Task Seed(CancellationToken cancellation = default(CancellationToken)) => Task.CompletedTask;
    }
}