using Database;
using Database.Entities;
using Utilities;

namespace Tests.Unit.Utilities
{
    public class TestIdentityObject : BaseEntity
    {
        public TestIdentityObject()
            : base(IdentityProvider.Instance)
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