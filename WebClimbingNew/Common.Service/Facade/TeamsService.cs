namespace Climbing.Web.Common.Service.Facade
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service.Exceptions;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Model;
    using Climbing.Web.Model.Facade;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    internal sealed class TeamsService : ITeamsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<TeamsService> logger;

        public TeamsService(IUnitOfWork unitOfWork, ILogger<TeamsService> logger)
        {
            Guard.NotNull(unitOfWork, nameof(unitOfWork));
            Guard.NotNull(logger, nameof(logger));

            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<TeamFacade> CreateTeam(string parentTeamCode, TeamFacade team, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.NotNull(parentTeamCode, nameof(parentTeamCode));

            var parentTeam = await this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.Code == parentTeamCode);
            if (parentTeam == null)
            {
                throw new ObjectNotFoundException($"Can't find team {parentTeamCode}");
            }

            var entity = new Team
            {
                Name = team.Name,
                Code = string.IsNullOrWhiteSpace(team.Code)
                        ? (await this.GenerateNextTeamCode(parentTeam, cancellationToken))
                        : team.Code,
                ParentId = parentTeam.Id,
            };

            var newE = await this.unitOfWork.Repository<Team>().AddAsync(entity, cancellationToken);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return new TeamFacade
            {
                Name = newE.Entity.Name,
                Code = newE.Entity.Code,
                Id = newE.Entity.Id,
            };
        }

        public Task<TeamFacade> GetRootTeam(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetTeam(Team.RootTeamCode, cancellationToken);
        }

        public async Task<TeamFacade> GetTeam(string teamCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.NotNull(teamCode, nameof(teamCode));

            var team = await this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.Code == teamCode);

            return team == null
                ? null
                : new TeamFacade
                {
                    Code = team.Code,
                    Id = team.Id,
                    Name = team.Name,
                };
        }

        public Task<IPagedCollection<TeamFacade>> GetTeams(IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetTeams(Team.RootTeamCode, paging, cancellationToken);
        }

        public async Task<IPagedCollection<TeamFacade>> GetTeams(string parentTeamCode, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            Guard.NotNull(parentTeamCode, nameof(parentTeamCode));

            Guard.NotNull(paging, nameof(paging));

            var parentTeam = await this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.Code == parentTeamCode);
            if (parentTeam == null)
            {
                throw new ObjectNotFoundException();
            }

            var data = await this.unitOfWork.Repository<Team>()
                            .Where(t => t.ParentId == parentTeam.Id)
                            .Select(t => new TeamFacade
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Code = t.Code,
                            })
                            .OrderBy(f => f.Code)
                            .ApplyPaging(paging, cancellationToken);
            return data;
        }

        private async Task<string> GenerateNextTeamCode(Team parentTeam, CancellationToken cancellationToken)
        {
            var childTeam = await this.unitOfWork.Repository<Team>()
                .Where(t => t.ParentId == parentTeam.Id)
                .OrderByDescending(t => t.Code)
                .FirstOrDefaultAsync(cancellationToken);

            var currentLocalCode = 0;
            string currentCode;
            do
            {
                currentCode = $"{parentTeam.Code}{++currentLocalCode:00}";
            }
            while (await this.unitOfWork.Repository<Team>().AnyAsync(t => t.Code == currentCode, cancellationToken));
            return currentCode;
        }
    }
}