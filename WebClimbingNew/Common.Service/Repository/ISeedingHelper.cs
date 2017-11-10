using System.Threading;
using System.Threading.Tasks;

namespace Climbing.Web.Common.Service.Repository
{
    public interface ISeedingHelper
    {
        Task<bool> IsSeeded(CancellationToken cancellationToken = default(CancellationToken));

        Task Seed(CancellationToken cancellation = default(CancellationToken));
    }
}