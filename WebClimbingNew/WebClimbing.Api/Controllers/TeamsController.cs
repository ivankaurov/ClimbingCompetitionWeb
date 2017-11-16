using System.Threading.Tasks;
using Climbing.Web.Api.Model;
using Climbing.Web.Api.Utilites;
using Climbing.Web.Common.Service.Exceptions;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Climbing.Web.Api.Controllers
{
    [Route("api/teams")]
    public class TeamsController : Controller
    {
        internal const string GetRootTeamsRouteName = "GetRootChildrenTeams";

        internal const string GetChildTeamsRouteName = "GetChildTeams";

        private readonly ITeamsService teamsService;
        private readonly IUrlHelper urlHelper;

        public TeamsController(ITeamsService teamsService, IUrlHelper urlHelper)
        {
            Guard.NotNull(teamsService, nameof(teamsService));
            Guard.NotNull(urlHelper, nameof(urlHelper));
            this.teamsService = teamsService;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = GetRootTeamsRouteName)]
        public async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            var response = await this.teamsService.GetTeams(pageParameters);
            return this.Ok(response.ToPagedResult(this.urlHelper, GetRootTeamsRouteName));
        }

        [HttpGet("{parent}/children", Name = GetChildTeamsRouteName)]
        public async Task<IActionResult> Get(string parent, [FromQuery] PageParameters pageParameters)
        {
            try
            {
                var response = await this.teamsService.GetTeams(parent, pageParameters);
                return this.Ok(response.ToPagedResult(this.urlHelper, GetChildTeamsRouteName));
            }
            catch(ObjectNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpGet("root")]
        public async Task<IActionResult> Get()
        {
            var team = await this.teamsService.GetRootTeam();
            if(team == null)
            {
                return this.NotFound();
            }
            
            return this.Ok(team);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var team = await this.teamsService.GetTeam(code);
            if(team == null)
            {
                return this.NotFound();
            }
            
            return this.Ok(team);
        }
    }
}