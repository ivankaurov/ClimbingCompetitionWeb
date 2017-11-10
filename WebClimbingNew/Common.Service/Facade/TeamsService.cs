using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Model.Facade;

namespace Climbing.Web.Common.Service.Facade
{
    internal sealed class TeamsService : ITeamsService
    {
        public Task<ICollection<TeamFacade>> GetTeams(IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetTeams(null, paging, cancellationToken);
        }

        public Task<ICollection<TeamFacade>> GetTeams(string parentTeamCode, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}