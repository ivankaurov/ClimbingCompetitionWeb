using System;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Utilities;
using Microsoft.Extensions.Logging;

namespace Climbing.Web.Common.Service
{
    internal sealed class MigrationWaitHelper : IMigrationWaitHelper
    {
        private readonly IContextHelper contextHelper;
        private readonly ILogger<MigrationWaitHelper> logger;

        public MigrationWaitHelper(IContextHelper contextHelper, ILogger<MigrationWaitHelper> logger)
        {
            Guard.NotNull(contextHelper, nameof(contextHelper));
            Guard.NotNull(logger, nameof(logger));

            this.contextHelper = contextHelper;
            this.logger = logger;
        }

        public async Task WaitForMigrationsToComplete(TimeSpan waitTimout, TimeSpan pollInterval, CancellationToken cancellationToken)
        {
            Guard.Requires(pollInterval > TimeSpan.Zero, nameof(pollInterval), "Value should be positive");
            Guard.Requires(waitTimout > TimeSpan.Zero, nameof(waitTimout), "Value should be positive");

            using(var timerCts = new CancellationTokenSource(waitTimout))
            using(var mixedCts = CancellationTokenSource.CreateLinkedTokenSource(timerCts.Token, cancellationToken))
            {
                try
                {
                    while(true)
                    {
                        if(await this.contextHelper.IsMigrated(cancellationToken))
                        {
                            this.logger.LogInformation("Database has latest wersion");
                            return;
                        }

                        this.logger.LogTrace("Database isn't migrated. Waiting for {0} for the next poll", pollInterval);

                        await Task.Delay(pollInterval);
                    }
                }
                catch(OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    this.logger.LogWarning("Exiting on timeout {0}", waitTimout);
                    throw new TimeoutException($"Database isn't migrated in {waitTimout}");
                }
            }
        }
    }
}