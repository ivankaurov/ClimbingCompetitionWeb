using System;
using System.Linq;
using Database.Entities;
using Database.Entities.Logging;
using Database.Services;
using Tests.Unit.Utilities;
using Xunit;

namespace Tests.Unit
{
    public class LtrObjectTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldNotCreateOnNullObject(IIdentityProvider<Guid> identityProvider)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LtrObject<Guid>(null, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldCreateLtrObject(IIdentityObject<Guid> obj, IIdentityProvider<Guid> identityProvider)
        {
            // Arrange & Act
            var sut = new LtrObject<Guid>(obj, identityProvider);

            // Assert
            Assert.Equal(obj.Id, sut.ObjectId);
            Assert.Equal(obj.GetType().FullName, sut.LogObjectClass);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddNewProperties(TestIdentityObject<Guid> obj, IIdentityProvider<Guid> identityProvider)
        {
            // Arrange
            var sut = new LtrObject<Guid>(obj, identityProvider);

            // Act
            sut.SetNewValues(obj, identityProvider);

            // Assert
            Assert.Equal(obj.StringProperty, sut[nameof(obj.StringProperty)].NewValue);
            Assert.Equal(obj.IntProperty.ToString(), sut[nameof(obj.IntProperty)].NewValue);
            Assert.Null(sut[nameof(obj.NullProperty)].NewValue);

            Assert.Null(sut[nameof(obj.IntProperty)].OldValue);
            Assert.Null(sut[nameof(obj.StringProperty)].OldValue);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddOldProperties(TestIdentityObject<Guid> obj, IIdentityProvider<Guid> identityProvider)
        {
            // Arrange
            var sut = new LtrObject<Guid>(obj, identityProvider);

            // Act
            sut.SetOldValues(obj, identityProvider);

            // Assert
            Assert.Equal(obj.StringProperty, sut[nameof(obj.StringProperty)].OldValue);
            Assert.Equal(obj.IntProperty.ToString(), sut[nameof(obj.IntProperty)].OldValue);
            Assert.Null(sut[nameof(obj.IntProperty)].NewValue);
            Assert.Null(sut[nameof(obj.StringProperty)].NewValue);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldSkipNonSerializedProperties(TestIdentityObject<Guid> obj, IIdentityProvider<Guid> identityProvider)
        {
            // Arrange
            var sut = new LtrObject<Guid>(obj, identityProvider);

            // Act
            sut.SetOldValues(obj, identityProvider);

            // Assert
            Assert.Null(sut[nameof(obj.NonSerializedProperty)]);
            Assert.Null(sut[nameof(obj.Id)]);
            Assert.Null(sut[nameof(obj.ObjectClass)]);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldSetNewAndOldProperties(TestIdentityObject<Guid> obj, IIdentityProvider<Guid> identityProvider, int newInt, string newString)
        {
            // Arrange
            var sut = new LtrObject<Guid>(obj, identityProvider);
            var startTime = DateTimeOffset.Now;
            var oldInt = obj.IntProperty;
            var oldString = obj.StringProperty;

            // Act
            sut.SetOldValues(obj, identityProvider);
            var expectedQuantity = sut.Properties.Count;
            obj.IntProperty = newInt;
            obj.StringProperty = newString;
            sut.SetNewValues(obj, identityProvider);

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