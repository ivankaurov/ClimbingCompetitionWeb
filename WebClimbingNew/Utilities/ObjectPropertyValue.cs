using System;

namespace Utilities
{
    public sealed class ObjectPropertyValue
    {
        public ObjectPropertyValue(Type type, object value)
        {
            Guard.NotNull(type, nameof(type));

            this.Type = type;
            this.Value = value;
        }
        public Type Type { get; }

        public object Value { get; }
    }
}
