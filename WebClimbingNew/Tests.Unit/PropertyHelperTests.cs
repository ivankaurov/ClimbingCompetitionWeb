using System;
using Utilities;
using Xunit;

namespace Tests.Unit
{
    public class PropertyHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldSetPrivateProperty(string expected, TestClass sut)
        {
            // Act
            sut.SetProperty(s => s.PrivateProperty, expected);

            // Assert
            Assert.Equal(expected, sut.PrivateProperty);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldSetReadOnlyField(string expected, TestClass sut)
        {
            // Act
            sut.SetProperty(s => s.readonlyField, expected);

            // Assert
            Assert.Equal(expected, sut.readonlyField);
        }

        public sealed class TestClass
        {
            public readonly string readonlyField = Guid.NewGuid().ToString();
            public string PrivateProperty { get; private set; }
        }
    } 
}