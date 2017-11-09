using System.Threading;
using System.Threading.Tasks;

namespace Climbing.Web.Common.Service
{
    public interface IContextHelper
    {
        Task<bool> IsMigrated(CancellationToken cancellationToken);

        Task Migrate(CancellationToken cancellationToken);
    }
}