using AutoFixture.Xunit2;

namespace Climbing.Web.Tests.Unit
{
    internal class AutoMoqInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineDataAttribute(params object[] values) : base(new AutoMoqDataAttribute(), values)
        {
        }
    }
}
