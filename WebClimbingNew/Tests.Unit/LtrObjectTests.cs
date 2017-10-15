using System;
using Database.Entities;
using Database.Entities.Logging;
using Database.Services;
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
    }
}