using System.Threading.Tasks;
using Climbing.Web.Common.Service.Exceptions;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Climbing.Web.Api.Controllers
{
    [Route("api/teams")]
    public class TeamsController : Controller
    {
        private readonly ITeamsService teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            Guard.NotNull(teamsService, nameof(teamsService));
            this.teamsService = teamsService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            var response = await this.teamsService.GetTeams(pageParameters);
            return this.Ok(response);
        }

        [HttpGet("{parent}/children")]
        public async Task<IActionResult> Get(string parent, [FromQuery] PageParameters pageParameters)
        {
            try
            {
                var response = await this.teamsService.GetTeams(parent, pageParameters);
                return this.Ok(response);
            }
            catch(ObjectNotFoundException)
            {
                return this.NotFound(parent);
            }
        }

        [HttpGet("root")]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(this.NotFound());
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            return await Task.FromResult(this.NotFound());
        }
    }
}