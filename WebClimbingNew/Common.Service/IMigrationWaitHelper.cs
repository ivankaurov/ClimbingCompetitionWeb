namespace Climbing.Web.Common.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMigrationWaitHelper
    {
        Task WaitForMigrationsToComplete(TimeSpan waitTimout, TimeSpan pollInterval, CancellationToken cancellationToken);
    }
}