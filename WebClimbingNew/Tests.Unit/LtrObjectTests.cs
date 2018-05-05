namespace Climbing.Web.Tests.Unit
{
    using System;
    using Climbing.Web.Model;
    using Climbing.Web.Model.Logging;
    using Climbing.Web.Tests.Unit.Utilities;
    using Xunit;

    public class LtrObjectTests
    {
        [Fact]
        public void ShouldNotCreateOnNullObject()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LtrObject(null));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldCreateLtrObject(IIdentityObject obj)
        {
            // Arrange & Act
            var sut = new LtrObject(obj);

            // Assert
            Assert.Equal(obj.Id, sut.ObjectId);
            Assert.Equal(obj.GetType().FullName, sut.LogObjectClass);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddProperties(TestIdentityObject obj)
        {
            // Arrange
            var sut = new LtrObject(obj);

            // Act
            sut.SetValues(obj);

            // Assert
            Assert.Equal(obj.StringProperty, sut[nameof(obj.StringProperty)].Value);
            Assert.Equal(obj.IntProperty.ToString(), sut[nameof(obj.IntProperty)].Value);
            Assert.Null(sut[nameof(obj.NullProperty)].Value);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldSkipNonSerializedProperties(TestIdentityObject obj)
        {
            // Arrange
            var sut = new LtrObject(obj);

            // Act
            sut.SetValues(obj);

            // Assert
            Assert.Null(sut[nameof(obj.NonSerializedProperty)]);
            Assert.Null(sut[nameof(obj.Id)]);
        }
    }
}