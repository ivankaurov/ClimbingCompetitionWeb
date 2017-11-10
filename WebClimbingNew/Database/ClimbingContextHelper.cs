using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service;
using Climbing.Web.Model;
using Climbing.Web.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climbing.Web.Database
{
    internal sealed class ClimbingContextHelper : IContextHelper
    {
        private readonly ClimbingContext context;

        private readonly ILogger logger;

        public ClimbingContextHelper(ClimbingContext climbingContext, ILogger<ClimbingContextHelper> logger)
        {
            Guard.NotNull(climbingContext, nameof(climbingContext));
            Guard.NotNull(logger, nameof(logger));
            this.context = climbingContext;
            this.logger = logger;
        }

        public async Task<bool> IsMigrated(CancellationToken cancellationToken)
        {
            this.logger.LogTrace(nameof(this.IsMigrated) + ": Enter");

            var result = !(await this.context.Database.GetPendingMigrationsAsync(cancellationToken)).Any();
            
            this.logger.LogInformation(nameof(this.IsMigrated) + ": Exit {0}", result);
            return result;
        }

        public async Task Migrate(CancellationToken cancellationToken) => await this.context.Database.MigrateAsync(cancellationToken);
    }
}