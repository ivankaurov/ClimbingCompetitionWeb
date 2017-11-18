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
        public void ShouldThrowOnDuplicateAddObject(TestIdentityObject obj, ChangeType changeTypeOne, ChangeType changeTypeTwo)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddObject(obj, changeTypeOne);

            // Arrange
            Assert.Throws<ArgumentException>(() => sut.AddObject(obj, changeTypeTwo));
        }

        [Theory]
        [AutoMoqData]
        public void ShouldAddObject(TestIdentityObject obj, ChangeType changeType)
        {
            // Arrange
            var sut = new Ltr();

            // Act
            sut.AddObject(obj, changeType);

            // Arrange
            var ltrObj = Assert.Single(sut.Objects);
            Assert.Equal(obj.IntProperty.ToString(), ltrObj[nameof(obj.IntProperty)].Value);
            Assert.Equal(obj.StringProperty, ltrObj[nameof(obj.StringProperty)].Value);
            Assert.Equal(changeType, ltrObj.ChangeType);
        }
    }
}