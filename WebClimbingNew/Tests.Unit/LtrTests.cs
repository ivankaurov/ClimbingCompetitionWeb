using System;
using Climbing.Web.Model.Logging;
using Climbing.Web.Tests.Unit.Utilities;
using Xunit;

namespace Climbing.Web.Tests.Unit
{
    public class LtrTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldAddNewObject(TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddNewObject(obj);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(obj.IntProperty.ToString(), ltrObj[nameof(obj.IntProperty)].NewValue);
            Assert.Equal(obj.StringProperty, ltrObj[nameof(obj.StringProperty)].NewValue);
            Assert.Null(ltrObj[nameof(obj.IntProperty)].OldValue);
            Assert.Equal(ChangeType.New,ltrObj.ChangeType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddNewObject(TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddNewObject(obj);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddNewObject(obj));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddDeletedObject(TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddDeletedObject(obj);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(obj.IntProperty.ToString(), ltrObj[nameof(obj.IntProperty)].OldValue);
            Assert.Equal(obj.StringProperty, ltrObj[nameof(obj.StringProperty)].OldValue);
            Assert.Null(ltrObj[nameof(obj.IntProperty)].NewValue);
            Assert.Equal(ChangeType.Delete, ltrObj.ChangeType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddDeletedObject(TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddDeletedObject(obj);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddDeletedObject(obj));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddChangedObject(TestIdentityObject obj, string newValue)
        {
            // Arrange
            var sut = new Ltr();
            var oldValue = obj.StringProperty;

            // Act
            sut.AddObjectBeforeChange(obj);
            obj.StringProperty = newValue;
            sut.AddObjectAfterChange(obj);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(oldValue, ltrObj[nameof(obj.StringProperty)].OldValue);
            Assert.Equal(newValue, ltrObj[nameof(obj.StringProperty)].NewValue);
            Assert.Equal(ChangeType.Update, ltrObj.ChangeType);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnNoOldValues(TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.AddObjectAfterChange(obj));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowOnDuplicateAddOldValuesObject(TestIdentityObject obj)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddDeletedObject(obj);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddObjectBeforeChange(obj));
        }
    }
}