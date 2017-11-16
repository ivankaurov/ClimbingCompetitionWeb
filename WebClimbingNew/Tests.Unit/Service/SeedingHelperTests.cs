using System;
using System.Threading.Tasks;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Common.Service.Repository;
using Climbing.Web.Database;
using Climbing.Web.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Climbing.Web.Tests.Unit.Service
{
    public class SeedingHelperTests : IDisposable
    {
        private readonly Mock<ILogger<TeamsService>> teamsServiceLoggerMock = new Mock<ILogger<TeamsService>>();

        private readonly Mock<ILogger<SeedingHelper>> seedingHelperLoggerMock = new Mock<ILogger<SeedingHelper>>();

        private readonly SeedingHelper seedingHelperSut;

        private readonly TeamsService teamsServiceSut;

        private readonly ClimbingContext context;

        public SeedingHelperTests()
        {
            this.context = new ClimbingContext(
                new DbContextOptionsBuilder<ClimbingContext>().UseInMemoryDatabase($"Db{Guid.NewGuid()}").Options);
            this.teamsServiceSut = new TeamsService(this.context, this.TeamsServiceLogger);
            this.seedingHelperSut = new SeedingHelper(this.context, this.teamsServiceSut, this.SeedingHelperLogger);
        }

        private ILogger<TeamsService> TeamsServiceLogger => this.teamsServiceLoggerMock.Object;

        private ILogger<SeedingHelper> SeedingHelperLogger => this.seedingHelperLoggerMock.Object;

        [Fact]
        public async Task ShouldSeedEmptyDatabaseOnlyOnce()
        {
            var seeded = await this.seedingHelperSut.IsSeeded();
            Assert.False(seeded);

            await this.seedingHelperSut.Seed();
            seeded = await this.seedingHelperSut.IsSeeded();
            Assert.True(seeded);

            await this.seedingHelperSut.Seed();
            seeded = await this.seedingHelperSut.IsSeeded();
            Assert.True(seeded);

            // Assert
            var rootTeam = await this.teamsServiceSut.GetRootTeam();
            Assert.NotNull(rootTeam);
            Assert.Equal(Team.RootTeamCode, rootTeam.Code);
            Assert.Equal(Team.RootTeamName, rootTeam.Name);

            var rootEntity = await this.context.Repository<Team>().SingleAsync(t => t.Id == rootTeam.Id);
            foreach(var t in SeedingHelper.SrcTeams)
            {
                await this.AssertTeamWithChildren(t, rootEntity);
            }
        }

        private async Task AssertTeamWithChildren(Team expected, Team parent)
        {
            var actual = await this.context.Repository<Team>().SingleAsync(t => t.Name == expected.Name && t.ParentId == parent.Id);
            Assert.Equal(expected.Name, actual.Name);
            if(expected.Code != null)
            {
                Assert.Equal(expected.Code, actual.Code);
            }
            else
            {
                Assert.NotEmpty(actual.Code);
            }

            foreach(var child in expected.Children)
            {
                await this.AssertTeamWithChildren(child, actual);
            }
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}