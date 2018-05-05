namespace Climbing.Web.Common.Service.Facade
{
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Model.Facade;
    using Climbing.Web.Utilities;

    public interface ITeamsService
    {
        Task<IPagedCollection<TeamFacade>> GetTeams(IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken));

        Task<IPagedCollection<TeamFacade>> GetTeams(string parentTeamCode, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken));

        Task<TeamFacade> GetTeam(string teamCode, CancellationToken cancellationToken = default(CancellationToken));

        Task<TeamFacade> GetRootTeam(CancellationToken cancellationToken = default(CancellationToken));

        Task<TeamFacade> CreateTeam(string parentTeamCode, TeamFacade team, CancellationToken cancellationToken = default(CancellationToken));
    }
}