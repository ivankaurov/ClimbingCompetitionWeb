namespace Climbing.Web.Utilities
{
    using System;

    public sealed class ObjectPropertyValue
    {
        public ObjectPropertyValue(Type type, object value, MemberType memberType)
        {
            Guard.NotNull(type, nameof(type));

            this.Type = type;
            this.Value = value;
            this.MemberType = memberType;
        }

        public Type Type { get; }

        public object Value { get; }

        public MemberType MemberType { get; }
    }
}
