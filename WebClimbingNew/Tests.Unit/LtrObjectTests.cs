using System;
using Database.Entities;
using Database.Entities.Logging;
using Tests.Unit.Utilities;
using Xunit;

namespace Tests.Unit
{
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
        public void ShouldAddNewProperties(TestIdentityObject obj)
        {
            // Arrange
            var sut = new LtrObject(obj);

            // Act
            sut.SetNewValues(obj);

            // Assert
            Assert.Equal(obj.StringProperty, sut[nameof(obj.StringProperty)].NewValue);
            Assert.Equal(obj.IntProperty.ToString(), sut[nameof(obj.IntProperty)].NewValue);
            Assert.Null(sut[nameof(obj.NullProperty)].NewValue);

            Assert.Null(sut[nameof(obj.IntProperty)].OldValue);
            Assert.Null(sut[nameof(obj.StringProperty)].OldValue);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddOldProperties(TestIdentityObject obj)
        {
            // Arrange
            var sut = new LtrObject(obj);

            // Act
            sut.SetOldValues(obj);

            // Assert
            Assert.Equal(obj.StringProperty, sut[nameof(obj.StringProperty)].OldValue);
            Assert.Equal(obj.IntProperty.ToString(), sut[nameof(obj.IntProperty)].OldValue);
            Assert.Null(sut[nameof(obj.IntProperty)].NewValue);
            Assert.Null(sut[nameof(obj.StringProperty)].NewValue);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldSkipNonSerializedProperties(TestIdentityObject obj)
        {
            // Arrange
            var sut = new LtrObject(obj);

            // Act
            sut.SetOldValues(obj);

            // Assert
            Assert.Null(sut[nameof(obj.NonSerializedProperty)]);
            Assert.Null(sut[nameof(obj.Id)]);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldSetNewAndOldProperties(TestIdentityObject obj, int newInt, string newString)
        {
            // Arrange
            var sut = new LtrObject(obj);
            var startTime = DateTimeOffset.Now;
            var oldInt = obj.IntProperty;
            var oldString = obj.StringProperty;

            // Act
            sut.SetOldValues(obj);
            var expectedQuantity = sut.Properties.Count;
            obj.IntProperty = newInt;
            obj.StringProperty = newString;
            sut.SetNewValues(obj);

            // Assert
            Assert.Equal(expectedQuantity, sut.Properties.Count);
            var intV = sut[nameof(obj.IntProperty)];
            var stringV = sut[nameof(obj.StringProperty)];

            Assert.Equal(oldInt.ToString(), intV.OldValue);
            Assert.Equal(newInt.ToString(), intV.NewValue);
            Assert.Equal(oldString, stringV.OldValue);
            Assert.Equal(newString, stringV.NewValue);
        }
    }
}