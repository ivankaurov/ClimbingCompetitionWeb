using System;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Climbing.Web.Tests.Unit
{
    public class MigrationWaitHelperTests
    {
        private static readonly TimeSpan DefaultWaitTimeSpan = TimeSpan.FromSeconds(10);

        private static readonly TimeSpan DefaultPollTimeSpan = TimeSpan.FromSeconds(1);

        private readonly Mock<ILogger<MigrationWaitHelper>> loggerMock = new Mock<ILogger<MigrationWaitHelper>>();

        private ILogger<MigrationWaitHelper> Logger => this.loggerMock.Object;

        [Theory]
        [AutoMoqData]
        public async Task ShouldReturnOnMigrated(Mock<IContextHelper> contextHelper)
        {
            // Assert
            contextHelper.Setup(c => c.IsMigrated(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var sut = new MigrationWaitHelper(contextHelper.Object, this.Logger);

            // Act
            await sut.WaitForMigrationsToComplete(DefaultWaitTimeSpan, DefaultPollTimeSpan, CancellationToken.None);

            // Assert
            contextHelper.Verify(c => c.IsMigrated(CancellationToken.None), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldWaitForMigrationToComplete(Mock<IContextHelper> contextHelper)
        {
            // Assert
            var completedAt = DateTimeOffset.Now.AddSeconds(5);
            contextHelper.Setup(c => c.IsMigrated(It.IsAny<CancellationToken>())).Returns<CancellationToken>(cts => Task.FromResult(DateTimeOffset.Now > completedAt));
            var sut = new MigrationWaitHelper(contextHelper.Object, this.Logger);

            // Act
            await sut.WaitForMigrationsToComplete(DefaultWaitTimeSpan, DefaultPollTimeSpan, CancellationToken.None);

            // Assert
            contextHelper.Verify(c => c.IsMigrated(CancellationToken.None), Times.AtLeast(3));
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldThrowTimeoutExceptionOnTimeout(Mock<IContextHelper> contextHelper)
        {
            // Assert
            contextHelper.Setup(c => c.IsMigrated(It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var sut = new MigrationWaitHelper(contextHelper.Object, this.Logger);

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(
                () =>sut.WaitForMigrationsToComplete(TimeSpan.FromSeconds(5), DefaultPollTimeSpan, CancellationToken.None));
            contextHelper.Verify(c => c.IsMigrated(CancellationToken.None), Times.AtLeast(2));
        }

        [Theory]
        [AutoMoqData]
        public async Task ShouldThrowOperationCancelledExceptionOnExternalCancellation(Mock<IContextHelper> contextHelper)
        {
            // Assert
            contextHelper.Setup(c => c.IsMigrated(It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var sut = new MigrationWaitHelper(contextHelper.Object, this.Logger);
            using(var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                // Act & Assert
                await Assert.ThrowsAnyAsync<OperationCanceledException>(
                    () => sut.WaitForMigrationsToComplete(DefaultWaitTimeSpan, DefaultPollTimeSpan, cts.Token));
                contextHelper.Verify(c => c.IsMigrated(cts.Token), Times.AtLeast(2));
            }
        }
    }
}