using Xunit;

namespace Climbing.Web.Tests.Unit.Api
{
    public class PagedResultFactoryTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldCreatePrevAndNextLinks()
        {
        }

        [Theory]
        [AutoMoqData]
        public void ShouldCreatePrevOnly()
        {
            
        }

        [Theory]
        [AutoMoqData]
        public void ShouldCreateNextOnly()
        {
        }

        [Theory]
        [AutoMoqData]
        public void ShouldCreateNoLinks()
        {
        }
    }
}