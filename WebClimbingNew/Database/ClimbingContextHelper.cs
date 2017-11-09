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
            var hasMigrations = (await this.context.Database.GetPendingMigrationsAsync(cancellationToken)).Any();
            if(hasMigrations)
            {
                return false;
            }

            return await this.IsSeeded(cancellationToken);
        }

        public async Task Migrate(CancellationToken cancellationToken)
        {
            await this.context.Database.MigrateAsync(cancellationToken);
            if(!(await this.IsSeeded(cancellationToken)))
            {
                try
                {
                    await this.Seed(cancellationToken);
                }
                catch(Exception ex)
                {
                    this.logger.LogError(ex, "Seeding failed. {0}", ex.Message);
                    throw;
                }
            }
        }

        private async Task<bool> IsSeeded(CancellationToken cancellationToken) =>
            await this.context.Teams.AnyAsync(t => t.Code == ClimbingContext.RootTeamCode && t.ParentId == null);

        private async Task Seed(CancellationToken cancellationToken)
        {
            await this.context.Teams.AddAsync(
                new Team { Name = "Россия", Code = ClimbingContext.RootTeamCode, ParentId = null },
                cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}