using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Api.Controllers;
using Climbing.Web.Api.Model;
using Climbing.Web.Common.Service.Exceptions;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Model.Facade;
using Climbing.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Climbing.Web.Tests.Unit.Api
{
    public class TeamsControllerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnTeams(Mock<ITeamsService> teamsService, PagedCollection<TeamFacade> expectedResult, PageParameters paging, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetTeams(paging, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var actionResult = await sut.Get(paging);

            // Assert
            var okactionRsult = Assert.IsType<OkObjectResult>(actionResult);
            var actual = Assert.IsAssignableFrom<PagedResult<TeamFacade>>(okactionRsult.Value);
            Assert.Equal(expectedResult, actual);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnEmptyTeams(Mock<ITeamsService> teamsService, PageParameters paging, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetTeams(paging, It.IsAny<CancellationToken>()))
                .ReturnsAsync(PagedCollection<TeamFacade>.Empty(paging.PageSize));
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var actionResult = await sut.Get(paging);

            // Assert
            var okactionRsult = Assert.IsType<OkObjectResult>(actionResult);
            var actual = Assert.IsAssignableFrom<PagedResult<TeamFacade>>(okactionRsult.Value);
            Assert.Empty(actual);

            // Should not create links
            Assert.Empty(actual.Links);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnTeamsByParent(Mock<ITeamsService> teamsService, string parent, PagedCollection<TeamFacade> expectedResult, PageParameters paging, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetTeams(parent, paging, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var actionResult = await sut.Get(parent, paging);

            // Assert
            var okactionRsult = Assert.IsType<OkObjectResult>(actionResult);
            var actual = Assert.IsAssignableFrom<PagedResult<TeamFacade>>(okactionRsult.Value);
            Assert.Equal(expectedResult, actual);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnEmptyTeamsByParent(Mock<ITeamsService> teamsService, PageParameters paging, string parent, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetTeams(parent, paging, It.IsAny<CancellationToken>()))
                .ReturnsAsync(PagedCollection<TeamFacade>.Empty(paging.PageSize));
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var actionResult = await sut.Get(parent, paging);

            // Assert
            var okactionRsult = Assert.IsType<OkObjectResult>(actionResult);
            var actual = Assert.IsAssignableFrom<PagedResult<TeamFacade>>(okactionRsult.Value);
            Assert.Empty(actual);

            // Should not create links
            Assert.Empty(actual.Links);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturn404OnIncorrectParentParent(Mock<ITeamsService> teamsService, PageParameters paging, string parent, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetTeams(parent, paging, It.IsAny<CancellationToken>())).ThrowsAsync(new ObjectNotFoundException());
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var actionResult = await sut.Get(parent, paging);

            // Assert
            var nfr = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal(parent, nfr.Value as string);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnTeamByCode(Mock<ITeamsService> teamsService, string code, TeamFacade expected, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetTeam(code, It.IsAny<CancellationToken>())).ReturnsAsync(expected);
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var res = await sut.Get(code);

            // Assert
            var okRes = Assert.IsType<OkObjectResult>(res);
            var actRes = Assert.IsType<TeamFacade>(okRes.Value);
            Assert.Equal(expected, actRes);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnRootTeam(Mock<ITeamsService> teamsService, TeamFacade expected, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetRootTeam(It.IsAny<CancellationToken>())).ReturnsAsync(expected);
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var res = await sut.Get();

            // Assert
            var okRes = Assert.IsType<OkObjectResult>(res);
            var actRes = Assert.IsType<TeamFacade>(okRes.Value);
            Assert.Equal(expected, actRes);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturn404OnMissingRoot(Mock<ITeamsService> teamsService, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetRootTeam(It.IsAny<CancellationToken>())).ReturnsAsync((TeamFacade)null);
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var res = await sut.Get();

            // Assert
            Assert.IsType<NotFoundResult>(res);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturn404OnIncorrectCode(Mock<ITeamsService> teamsService, string code, Mock<IUrlHelper> urlHelper)
        {
            // Arrange
            teamsService.Setup(s => s.GetRootTeam(It.IsAny<CancellationToken>())).ReturnsAsync((TeamFacade)null);
            var sut = new TeamsController(teamsService.Object, urlHelper.Object);

            // Act
            var res = await sut.Get(code);

            // Assert
            var nfr = Assert.IsType<NotFoundObjectResult>(res);
            var act = Assert.IsType<string>(nfr.Value);
            Assert.Equal(act, code);
        }
    }
}