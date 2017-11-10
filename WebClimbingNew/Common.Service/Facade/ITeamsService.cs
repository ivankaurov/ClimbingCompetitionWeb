using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Model.Facade;

namespace Climbing.Web.Common.Service.Facade
{
    public interface ITeamsService
    {
        Task<ICollection<TeamFacade>> GetTeams(IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken));

        Task<ICollection<TeamFacade>> GetTeams(string parentTeamCode, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken));
    }
}