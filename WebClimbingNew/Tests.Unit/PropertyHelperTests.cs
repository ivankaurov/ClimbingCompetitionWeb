using System;
using Climbing.Web.Tests.Unit.Utilities;
using Climbing.Web.Utilities;
using Xunit;

namespace Climbing.Web.Tests.Unit
{
    public class PropertyHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldSetPrivateProperty(string expected, TestClass sut, TestIdentityObject sut2, DateTimeOffset cr, DateTimeOffset ch, Guid id)
        {
            // Act
            sut.SetProperty(s => s.PrivateProperty, expected);
            sut.SetProperty(s => s.PublicProperty, expected);
            sut2.SetProperty(s => s.WhenCreated, cr);
            sut2.SetProperty(s => s.WhenChanged, ch);
            sut2.SetProperty(s => s.Id, id);

            // Assert
            Assert.Equal(expected, sut.PrivateProperty);
            Assert.Equal(cr, sut2.WhenCreated);
            Assert.Equal(ch, sut2.WhenChanged);
            Assert.Equal(id, sut2.Id);
            Assert.Equal(expected, sut.PublicProperty);
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

            public string PublicProperty { get; set; }
        }
    } 
}