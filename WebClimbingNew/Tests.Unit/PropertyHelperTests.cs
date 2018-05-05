namespace Climbing.Web.Tests.Unit
{
    using System;
    using Climbing.Web.Tests.Unit.Utilities;
    using Climbing.Web.Utilities;
    using Xunit;

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
            sut.SetProperty(s => s.ReadonlyField, expected);

            // Assert
            Assert.Equal(expected, sut.ReadonlyField);
        }

        public sealed class TestClass
        {
#pragma warning disable SA1401 // Fields should be private
            public readonly string ReadonlyField = Guid.NewGuid().ToString();
#pragma warning restore SA1401 // Fields should be private

            public string PrivateProperty { get; private set; }

            public string PublicProperty { get; set; }
        }
    }
}