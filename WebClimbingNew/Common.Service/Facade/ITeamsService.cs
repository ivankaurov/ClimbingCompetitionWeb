using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Model.Facade;
using Climbing.Web.Utilities;

namespace Climbing.Web.Common.Service.Facade
{
    public interface ITeamsService
    {
        Task<IPagedCollection<TeamFacade>> GetTeams(IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken));

        Task<IPagedCollection<TeamFacade>> GetTeams(string parentTeamCode, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken));
    }
}