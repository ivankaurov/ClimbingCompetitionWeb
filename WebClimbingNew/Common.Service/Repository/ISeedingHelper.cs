namespace Climbing.Web.Common.Service.Repository
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISeedingHelper
    {
        Task<bool> IsSeeded(CancellationToken cancellationToken = default(CancellationToken));

        Task Seed(CancellationToken cancellation = default(CancellationToken));
    }
}