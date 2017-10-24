using System;
using System.Collections.Generic;
using System.Linq;
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
            var results = new HashSet<string>(idCount, StringComparer.OrdinalIgnoreCase);
            sut.CreateNewIdentity();

            // Act
            Parallel.For(0, idCount, n => Assert.True(results.Add(sut.CreateNewIdentity())));

            // Assert
            Assert.Equal(idCount, results.Count);
        }
    }
}