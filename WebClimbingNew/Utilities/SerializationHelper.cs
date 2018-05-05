namespace Climbing.Web.Utilities
{
    using System;
    using System.Collections.Concurrent;

    public static class SerializationHelper
    {
        private static readonly ConcurrentDictionary<Type, bool> SerializableTypes = new ConcurrentDictionary<Type, bool>();

        public static bool ShouldSerialize(this object obj)
        {
            Guard.NotNull(obj, nameof(obj));

            return SerializableTypes.GetOrAdd(
                obj.GetType(),
                t => !Attribute.IsDefined(t, typeof(SerializeSkipAttribute)));
        }
    }
}