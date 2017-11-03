using System;
using Climbing.Web.Utilities;
using Xunit;

namespace Climbing.Web.Tests.Unit
{
    public class MethodHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldExtractReferenceProperty(string propertyValueStr)
        {
            // Arrange
            var sut = new
            {
                TestPropertyString = propertyValueStr
            };

            // Act
            var actual = (string)MethodHelper.GetPropertyOrFieldValue(sut, nameof(sut.TestPropertyString));

            // Assert
            Assert.Equal(propertyValueStr, actual);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldExtractValueProperty(int propertyValue)
        {
            // Arrange
            var sut = new
            {
                TestProperty = propertyValue
            };

            // Act
            var actual = (int)MethodHelper.GetPropertyOrFieldValue(sut, nameof(sut.TestProperty));

            // Assert
            Assert.Equal(propertyValue, actual);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldExtractPrivateField(int fieldValue)
        {
            // Arrange
            var sut = new TestClassWithPrivateField(fieldValue);

            // Act
            var actual = (int)MethodHelper.GetPropertyOrFieldValue(sut, "field");

            // Assert
            Assert.Equal(fieldValue, actual);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldThrowArgumentExceptionOnIncorrectProperty(TestClassWithPrivateField sut, string propertyName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => MethodHelper.GetPropertyOrFieldValue(sut, propertyName));
        }

        public sealed class TestClassWithPrivateField
        {
            private readonly int field;

            public TestClassWithPrivateField(int field)
            {
                this.field = field;
            }
        } 
    }
}
