namespace Climbing.Web.Tests.Unit
{
    using System.Linq;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service.Exceptions;
    using Climbing.Web.Common.Service.Facade;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Model;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class PagingAndTeamsServiceTests
    {
        private readonly Mock<ILogger<TeamsService>> loggerMock = new Mock<ILogger<TeamsService>>();

        private ILogger<TeamsService> Logger => this.loggerMock.Object;

        [Theory]
        [AutoMoqData]
        public async Task ShouldThrowObjectNotFoundExceptionOnNotFoundColde(IUnitOfWork unitOfWork, IPageParameters paging, string code)
        {
            // Arrange
            var sut = new TeamsService(unitOfWork, this.Logger);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => sut.GetTeams(code, paging));
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnEmptyPaging(IPageParameters pageParameters, IUnitOfWork uow)
        {
            // Arrange
            var sut = new TeamsService(uow, this.Logger);

            // Act
            var actual = await sut.GetTeams(uow.Repository<Team>().First(t => t.Code != Team.RootTeamCode).Code, pageParameters);

            // Assert
            Assert.Empty(actual);
            Assert.Equal(0, actual.PageNumber);
            Assert.Equal(0, actual.TotalPages);
            Assert.Equal(pageParameters.PageSize, actual.PageSize);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnPaging(IUnitOfWork uow)
        {
            // Arrange
            var sut = new TeamsService(uow, this.Logger);
            var pageParameters = new PageParameters { PageNumber = 2, PageSize = 3 };
            var expected = Enumerable.Range(4, 3);

            // Act
            var actual = await sut.GetTeams(Team.RootTeamCode, pageParameters);

            // Assert
            var zip = actual.Zip(expected, (a, e) => new { Actual = a, Expcted = e });
            Assert.All(zip, z => Assert.Equal(z.Expcted.ToString("00"), z.Actual.Code));
            Assert.All(zip, z => Assert.Equal($"Team_{z.Expcted:00}", z.Actual.Name));
            Assert.Equal(pageParameters.PageNumber, actual.PageNumber);
            Assert.Equal(4, actual.TotalPages);
            Assert.Equal(pageParameters.PageSize, actual.PageSize);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnLastPage(IUnitOfWork uow)
        {
            // Arrange
            var sut = new TeamsService(uow, this.Logger);
            var pageParameters = new PageParameters { PageNumber = 4, PageSize = 3 };

            // Act
            var actual = await sut.GetTeams(pageParameters);

            // Assert
            Assert.Equal(2, actual.Page.Count);
            Assert.Equal(pageParameters.PageNumber, actual.PageNumber);
            Assert.Equal(4, actual.TotalPages);
            Assert.Equal(pageParameters.PageSize, actual.PageSize);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnPageAfterLast(IUnitOfWork uow)
        {
            // Arrange
            var sut = new TeamsService(uow, this.Logger);
            var pageParameters = new PageParameters { PageNumber = 40, PageSize = 11 };

            // Act
            var actual = await sut.GetTeams(pageParameters);

            // Assert
            Assert.Empty(actual);
            Assert.Equal(2, actual.PageNumber);
            Assert.Equal(1, actual.TotalPages);
            Assert.Equal(pageParameters.PageSize, actual.PageSize);
        }
    }
}