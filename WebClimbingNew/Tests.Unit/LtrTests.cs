using System;
using Database.Entities.Logging;
using Database.Services;
using Tests.Unit.Utilities;
using Xunit;

namespace Tests.Unit
{
    public class LtrTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldAddNewObject(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);

            // Act
            sut.AddNewObject(obj, identityProvider);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(obj.IntProperty.ToString(), ltrObj[nameof(obj.IntProperty)].NewValue);
            Assert.Equal(obj.StringProperty, ltrObj[nameof(obj.StringProperty)].NewValue);
            Assert.Null(ltrObj[nameof(obj.IntProperty)].OldValue);
            Assert.Equal(ChangeType.New,ltrObj.ChangeType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddNewObject(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);

            // Act
            sut.AddNewObject(obj, identityProvider);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddNewObject(obj, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddDeletedObject(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);

            // Act
            sut.AddDeletedObject(obj, identityProvider);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(obj.IntProperty.ToString(), ltrObj[nameof(obj.IntProperty)].OldValue);
            Assert.Equal(obj.StringProperty, ltrObj[nameof(obj.StringProperty)].OldValue);
            Assert.Null(ltrObj[nameof(obj.IntProperty)].NewValue);
            Assert.Equal(ChangeType.Delete, ltrObj.ChangeType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddDeletedObject(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);

            // Act
            sut.AddDeletedObject(obj, identityProvider);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddDeletedObject(obj, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddChangedObject(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj, string newValue)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);
            var oldValue = obj.StringProperty;

            // Act
            sut.AddObjectBeforeChange(obj, identityProvider);
            obj.StringProperty = newValue;
            sut.AddObjectAfterChange(obj, identityProvider);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(oldValue, ltrObj[nameof(obj.StringProperty)].OldValue);
            Assert.Equal(newValue, ltrObj[nameof(obj.StringProperty)].NewValue);
            Assert.Equal(ChangeType.Update, ltrObj.ChangeType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnNoOldValues(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.AddObjectAfterChange(obj, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddOldValuesObject(IIdentityProvider<Guid> identityProvider, TestIdentityObject<Guid> obj)
        {
            // Arrange
            var sut = new Ltr<Guid>(identityProvider);

            // Act
            sut.AddDeletedObject(obj, identityProvider);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddObjectBeforeChange(obj, identityProvider));
        }
    }
}