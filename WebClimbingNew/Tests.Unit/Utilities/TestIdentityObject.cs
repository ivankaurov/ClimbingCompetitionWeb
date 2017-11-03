using Climbing.Web.Model;
using Climbing.Web.Utilities;

namespace Climbing.Web.Tests.Unit.Utilities
{
    public class TestIdentityObject : BaseEntity
    {
        public TestIdentityObject()
        {
            this.NullProperty = null;
        }

        public int IntProperty { get; set; }

        public string StringProperty { get; set; }

        [SerializeSkip]
        public string NonSerializedProperty { get; set; }

        public object NullProperty { get; }
    }
}