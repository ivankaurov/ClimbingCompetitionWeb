namespace Climbing.Web.Common.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Utilities;
    using Microsoft.Extensions.Logging;

    internal sealed class MigrationWaitHelper : IMigrationWaitHelper
    {
        private readonly IContextHelper contextHelper;
        private readonly ISeedingHelper seeder;
        private readonly ILogger<MigrationWaitHelper> logger;

        public MigrationWaitHelper(IContextHelper contextHelper, ISeedingHelper seeder, ILogger<MigrationWaitHelper> logger)
        {
            Guard.NotNull(contextHelper, nameof(contextHelper));
            Guard.NotNull(seeder, nameof(seeder));
            Guard.NotNull(logger, nameof(logger));

            this.contextHelper = contextHelper;
            this.seeder = seeder;
            this.logger = logger;
        }

        public async Task WaitForMigrationsToComplete(TimeSpan waitTimout, TimeSpan pollInterval, CancellationToken cancellationToken)
        {
            Guard.Requires(pollInterval > TimeSpan.Zero, nameof(pollInterval), "Value should be positive");
            Guard.Requires(waitTimout > TimeSpan.Zero, nameof(waitTimout), "Value should be positive");

            using (var timerCts = new CancellationTokenSource(waitTimout))
            using (var mixedCts = CancellationTokenSource.CreateLinkedTokenSource(timerCts.Token, cancellationToken))
            {
                try
                {
                    this.logger.LogInformation("Waiting for migrations to complete.");
                    await this.WaitForTaskToComplete(ct => this.contextHelper.IsMigrated(ct), pollInterval, mixedCts.Token);

                    this.logger.LogInformation("Waiting for seeding to complete");
                    await this.WaitForTaskToComplete(ct => this.seeder.IsSeeded(ct), pollInterval, mixedCts.Token);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    this.logger.LogWarning("Exiting on timeout {0}", waitTimout);
                    throw new TimeoutException($"Database isn't migrated in {waitTimout}");
                }
            }
        }

        private async Task WaitForTaskToComplete(Func<CancellationToken, Task<bool>> completionPollFunc, TimeSpan pollInterval, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (await completionPollFunc(cancellationToken))
                {
                    this.logger.LogInformation("Task complete.");
                    return;
                }

                this.logger.LogTrace("Task not completed. Next request in {0}", pollInterval);
                await Task.Delay(pollInterval, cancellationToken);
            }
        }
    }
}