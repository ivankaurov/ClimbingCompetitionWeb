using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service.Repository;

namespace Climbing.Web.Tests.Unit.Utilities
{
    internal sealed class SimpleSeedingHelper : ISeedingHelper
    {
        public Task<bool> IsSeeded(CancellationToken cancellationToken = default(CancellationToken)) => Task.FromResult(true);

        public Task Seed(CancellationToken cancellation = default(CancellationToken)) => Task.CompletedTask;
    }
}