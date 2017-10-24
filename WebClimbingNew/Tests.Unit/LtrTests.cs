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
        public void ShouldAddNewObject(IIdentityProvider identityProvider, TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr(identityProvider);

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
        public void ShouldThrowOnDuplicateAddNewObject(IIdentityProvider identityProvider, TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr(identityProvider);

            // Act
            sut.AddNewObject(obj, identityProvider);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddNewObject(obj, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddDeletedObject(IIdentityProvider identityProvider, TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr(identityProvider);

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
        public void ShouldThrowOnDuplicateAddDeletedObject(IIdentityProvider identityProvider, TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr (identityProvider);

            // Act
            sut.AddDeletedObject(obj, identityProvider);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddDeletedObject(obj, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddChangedObject(IIdentityProvider identityProvider, TestIdentityObject obj, string newValue)
        {
            // Arrange
            var sut = new Ltr (identityProvider);
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
        public void ShouldThrowOnNoOldValues(IIdentityProvider identityProvider, TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr(identityProvider);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.AddObjectAfterChange(obj, identityProvider));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddOldValuesObject(IIdentityProvider identityProvider, TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr(identityProvider);

            // Act
            sut.AddDeletedObject(obj, identityProvider);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddObjectBeforeChange(obj, identityProvider));
        }
    }
}