using System;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Model;
using Climbing.Web.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climbing.Web.Common.Service.Repository
{
    internal sealed class SeedingHelper : ISeedingHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<SeedingHelper> logger;

        public SeedingHelper(IUnitOfWork unitOfWork, ILogger<SeedingHelper> logger)
        {
            Guard.NotNull(unitOfWork, nameof(unitOfWork));
            Guard.NotNull(logger, nameof(logger));

            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        
        public async Task<bool> IsSeeded(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogTrace(nameof(this.IsSeeded) + ": Enter");

            var result = await this.unitOfWork.Repository<Team>().AnyAsync(t => t.Code == Team.RootTeamCode, cancellationToken);

            this.logger.LogTrace(nameof(this.IsSeeded) + ": Exit {0}", result);
            return result;
        }

        public async Task Seed(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogTrace(nameof(this.Seed) + ": Enter");

            if(await this.IsSeeded(cancellationToken))
            {
                this.logger.LogInformation(nameof(this.Seed) + ": Database already seeded.");
                return;
            }

            try
            {
                await this.unitOfWork.Repository<Team>().AddAsync(new Team{ Name = Team.RootTeamName, Code = Team.RootTeamCode }, cancellationToken);
                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                this.logger.LogInformation(nameof(this.Seed) + ": Seeding completed.");
            }
            catch(OperationCanceledException ex)
            {
                this.logger.LogWarning(ex, "Seeding cancelled");
                throw;
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, "Seeding failed: {0}", ex.Message);
                throw;
            }
        }
    }
}