using Database.Services;
using System;
using Utilities;
using Xunit;
using Database.Entities.Logging;

namespace Tests.Unit
{
    public class LtrObjectPropertiesTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldCreateWithNewValueSet(ObjectPropertyValue value, IIdentityProvider<Guid> identityProvider)
        {
            // Act
            var actual = LtrObjectProperties<Guid>.CreateWithNewValue(value, identityProvider);

            // Assert
            Assert.Equal(value.Type.FullName, actual.PropertyType);
            Assert.Equal(value.Value.ToString(), actual.NewValue);
            Assert.Null(actual.OldValue);
        }

        [Theory]
        [AutoMoqData]
        public void ShouldCreateWithOldValueSet(ObjectPropertyValue value, IIdentityProvider<Guid> identityProvider)
        {
            // Act
            var actual = LtrObjectProperties<Guid>.CreateWithOldValue(value, identityProvider);

            // Assert
            Assert.Equal(value.Type.FullName, actual.PropertyType);
            Assert.Equal(value.Value.ToString(), actual.OldValue);
            Assert.Null(actual.NewValue);
        }
    }
}