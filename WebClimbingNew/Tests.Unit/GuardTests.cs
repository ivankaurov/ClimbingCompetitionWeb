namespace Climbing.Web.Tests.Unit
{
    using System;
    using Climbing.Web.Utilities;
    using Xunit;

    public class GuardTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldPass(string value)
        {
            // Act
            Guard.NotNull(value, nameof(value));
            Guard.NotNullOrWhitespace(value, nameof(value));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowArgumentNullException(string parameterName)
        {
            // Act
            var actual = Assert.Throws<ArgumentNullException>(() => Guard.NotNull(null, parameterName));

            // Assert
            Assert.Equal(parameterName, actual.ParamName);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowExceptionWithMessage(string parameterName, string message)
        {
            // Act
            var actual = Assert.Throws<ArgumentNullException>(() => Guard.NotNull(null, parameterName, message));

            // Assert
            Assert.Equal(parameterName, actual.ParamName);
            Assert.Contains(message, actual.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowExceptionWithValue(string parameterName)
        {
            // Act
            var actual = Assert.Throws<ArgumentNullException>(() => Guard.NotNull(null, parameterName));

            // Assert
            Assert.Equal("value", actual.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowOnEmptyString(string value)
        {
            // Arrange
            var parameterName = Guid.NewGuid().ToString();

            // Act
            var actual = Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrWhitespace(value, parameterName));

            // Assert
            Assert.Equal(parameterName, actual.ParamName);
        }
    }
}
