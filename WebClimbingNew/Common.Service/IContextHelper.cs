namespace Climbing.Web.Common.Service
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IContextHelper
    {
        Task<bool> IsMigrated(CancellationToken cancellationToken);

        Task Migrate(CancellationToken cancellationToken);
    }
}