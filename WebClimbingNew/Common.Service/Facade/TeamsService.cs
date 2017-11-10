using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service.Exceptions;
using Climbing.Web.Common.Service.Repository;
using Climbing.Web.Model;
using Climbing.Web.Model.Facade;
using Climbing.Web.Utilities;
using Climbing.Web.Utilities.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climbing.Web.Common.Service.Facade
{
    internal sealed class TeamsService : ITeamsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<TeamsService> logger;

        static TeamsService()
        {
            AutoMapper.Setup((Team t) => t.Id).To((TeamFacade f) => f.Id);
            AutoMapper.Setup((Team t) => t.Code).To((TeamFacade f) => f.Code);
            AutoMapper.Setup((Team t) => t.Name).To((TeamFacade f) => f.Name);
        }

        public TeamsService(IUnitOfWork unitOfWork, ILogger<TeamsService> logger)
        {
            Guard.NotNull(unitOfWork, nameof(unitOfWork));
            Guard.NotNull(logger, nameof(logger));

            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public Task<IPagedCollection<TeamFacade>> GetTeams(IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetTeams(null, paging, cancellationToken);
        }

        public async Task<IPagedCollection<TeamFacade>> GetTeams(string parentTeamCode, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(string.IsNullOrWhiteSpace(parentTeamCode))
            {
                parentTeamCode = Team.RootTeamCode;
            }

            Guard.NotNull(paging, nameof(paging));

            var parentTeam = await this.unitOfWork.Repository<Team>().FirstOrDefaultAsync(t => t.Code == parentTeamCode);
            if(parentTeam == null)
            {
                throw new ObjectNotFoundException();
            }

            var data = await this.unitOfWork.Repository<Team>()
                            .Where(t => t.ParentId != null)
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
    }
}