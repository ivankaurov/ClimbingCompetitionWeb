using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Xunit;

namespace Tests.Unit
{
    public class IdentityProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldCreateUniqueId(int idCount)
        {
            // Arrange
            var sut = IdentityProvider.Instance;
            var results = new HashSet<Guid>(idCount);

            // Act
            Parallel.For(0, idCount, n => results.Add(sut.CreateNewIdentity()));

            // Assert
            Assert.Equal(idCount, results.Count);
        }
    }
}