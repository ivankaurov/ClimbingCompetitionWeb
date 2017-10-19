using Database.Entities;
using Utilities;

namespace Tests.Unit.Utilities
{
    public class TestIdentityObject<T> : BaseEntity<T>
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