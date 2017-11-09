using System;
using System.Threading;
using System.Threading.Tasks;

namespace Climbing.Web.Common.Service
{
    public interface IMigrationWaitHelper
    {
        Task WaitForMigrationsToComplete(TimeSpan waitTimout, TimeSpan pollInterval, CancellationToken cancellationToken);
    }
}