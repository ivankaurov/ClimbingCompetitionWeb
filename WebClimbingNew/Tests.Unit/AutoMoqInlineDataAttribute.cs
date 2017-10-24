using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit
{
    internal class AutoMoqInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineDataAttribute(params object[] values) : base(new AutoMoqDataAttribute(), values)
        {
        }
    }
}
