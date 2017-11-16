using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Model;
using Climbing.Web.Model.Facade;
using Climbing.Web.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climbing.Web.Common.Service.Repository
{
    internal sealed class SeedingHelper : ISeedingHelper
    {
        internal static readonly Team[] SrcTeams =
            {
                new Team { Name = "Москва", Code = "77" },
                new Team { Name = "Респ. Башкортостан", Code = "02" },
                new Team { Name = "Воронежская обл.", Code = "36" },
                new Team { Name = "Свердловская обл.", Code = "66" },
                new Team { Name = "ЯНАО", Code = "89" },
                new Team
                { 
                    Name = "Санкт-Петербург",
                    Code = "78",
                    Children =
                    {
                        new Team { Name = "Балт. Берег"},
                        new Team { Name = "ЦСКА" },
                        new Team { Name = "ЦФКСиЗ" },
                        new Team { Name = "шк.495" }
                    },
                }
            };
        private readonly IUnitOfWork unitOfWork;
        private readonly ITeamsService teamsService;
        private readonly ILogger<SeedingHelper> logger;

        public SeedingHelper(IUnitOfWork unitOfWork, ITeamsService teamsService, ILogger<SeedingHelper> logger)
        {
            Guard.NotNull(unitOfWork, nameof(unitOfWork));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(teamsService, nameof(teamsService));

            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.teamsService = teamsService;
        }
        
        public async Task<bool> IsSeeded(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogTrace(nameof(this.IsSeeded) + ": Enter");

            var rootTeam = await this.GetRootTeam(cancellationToken);
            var result = true;
            if(rootTeam == null)
            {
                this.logger.LogInformation("Root team missing");
                return false;
            }

            foreach(var t in SrcTeams)
            {
                var teamCreated = await this.TeamAndChildrenExist(t, rootTeam, cancellationToken);
                result &= teamCreated;
            }

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
                var rootTeam = await this.GetRootTeam(cancellationToken);
                if(rootTeam == null)
                {
                    rootTeam = await this.CreateRootTeam(cancellationToken);
                }

                foreach(var t in SrcTeams)
                {
                    await this.CreateTeamAndChildren(t, rootTeam, cancellationToken);
                }

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

        private async Task<bool> TeamAndChildrenExist(Team team, Team parent, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Checking team {0} {1} {2}", team.Code, team.Name, parent.Name);

            var foundTeam = team.Code == null
                ? await this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.ParentId == parent.Id && t.Name == team.Name, cancellationToken)
                : await this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.Code == team.Code, cancellationToken);

            if(foundTeam == null)
            {
                this.logger.LogInformation("Team {0} {1} {2} not found", team.Code, team.Name, parent.Name);
                return false;
            }

            if(team.Children.Count > 0)
            {
                this.logger.LogInformation("Checking children for {0}", foundTeam.Name);
                foreach (var item in team.Children)
                {
                    if(!(await this.TeamAndChildrenExist(item, foundTeam, cancellationToken)))
                    {
                        this.logger.LogInformation("Child {0} for team {1} not exists", item.Name, foundTeam.Name);
                        return false;
                    }
                }
            }

            this.logger.LogInformation("Team {0} {1} and all children (if required) exist. Id={2}", foundTeam.Code, foundTeam.Name, foundTeam.Id);
            return true;
        }

        private Task<Team> GetRootTeam(CancellationToken cancellationToken)
            => this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.Code == Team.RootTeamCode, cancellationToken);

        private async Task<Team> CreateRootTeam(CancellationToken cancellationToken)
        {
            var entity = new Team { Name = Team.RootTeamName, Code = Team.RootTeamCode };
            var creationResult = await this.unitOfWork.Repository<Team>().AddAsync(entity, cancellationToken);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return creationResult.Entity;
        }

        private async Task<Team> CreateTeamAndChildren(Team teamToCreate, Team parent, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Creating team {0} {1}", teamToCreate.Code, teamToCreate.Name);
            var resultFacade = await this.teamsService.CreateTeam(
                parent.Code,
                new TeamFacade
                {
                    Name = teamToCreate.Name,
                    Code = teamToCreate.Code
                },
                cancellationToken);

            var result = await this.unitOfWork.Repository<Team>().SingleAsync(t => t.Id == resultFacade.Id, cancellationToken);

            this.logger.LogInformation("Team {0} {1} {2} created.", result.Code, result.Name, result.Id);

            foreach(var child in teamToCreate.Children.OrderBy(c => c.Name))
            {
                await this.CreateTeamAndChildren(child, result, cancellationToken);
            }

            this.logger.LogInformation("Children for team {0} {1} created (if required)", result.Code, result.Name);

            return result;
        }
    }
}