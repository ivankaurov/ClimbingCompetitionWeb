using System.Linq;
using Climbing.Web.Api.Model;
using Climbing.Web.Api.Utilites;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Climbing.Web.Tests.Unit.Api
{
    public class PagedResultFactoryTests
    {
        [Theory]
        [AutoMoqInlineData(1, 1, false, false)]
        [AutoMoqInlineData(1, 2, false, true)]
        [AutoMoqInlineData(2, 2, true, false)]
        [AutoMoqInlineData(2, 3, true, true)]
        public void ShouldCreateLinks(int pageNumber, int totalPages, bool needPrev, bool needNext, Mock<IUrlHelper> urlHelper, string routeName, int pageSize, int[] pageData)
        {
            // Arrange
            if(pageSize > PageParameters.MaxPageSize)
            {
                pageSize = PageParameters.MaxPageSize;
            }

            var pagedCollection = new PagedCollection<int>(pageData, pageNumber, totalPages, pageSize);
            var expectedCount = (needPrev && needNext) ? 2 : ((needPrev || needNext) ? 1 : 0);

            // Act
            var result = pagedCollection.ToPagedResult(urlHelper.Object, routeName);

            // Assert
            Assert.Equal(needPrev, result.Links.ContainsKey(LinkType.PreviousPage));
            Assert.Equal(needNext, result.Links.ContainsKey(LinkType.NextPage));
            Assert.Equal(expectedCount, result.Links.Count);
            urlHelper.Verify(
                s => s.Link(routeName, It.Is<PageParameters>(p => p.PageNumber == pageNumber-1 && p.PageSize == pageSize)),
                needPrev ? Times.Once() : Times.Never());
            urlHelper.Verify(
                s => s.Link(routeName, It.Is<PageParameters>(p => p.PageNumber == pageNumber+1 && p.PageSize == pageSize)),
                needNext ? Times.Once() : Times.Never());
            Assert.All(result.Links, lnk => Assert.Equal("GET", lnk.Value.Method));
        }
    }
}