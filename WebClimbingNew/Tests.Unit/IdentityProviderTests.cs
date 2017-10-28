using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Xunit;

namespace Tests.Unit
{
    public class IdentityProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldCreateUniqueId(ValueGenerator<string> sut, int idCount)
        {
            // Arrange
            var results = new ConcurrentBag<string>();

            // Act
            Parallel.For(0, idCount, n => results.Add(sut.Next(null)));

            // Assert
            Assert.Equal(results.Count, results.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }
    }
}