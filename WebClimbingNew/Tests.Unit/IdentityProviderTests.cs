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
            var results = new HashSet<Guid>(idCount);
            sut.CreateNewIdentity();

            // Act
            for (int i = 0; i < idCount; i++)
            {
                results.Add(sut.CreateNewIdentity());
            }

            // Assert
            Assert.Equal(idCount, results.Count);
        }
    }
}