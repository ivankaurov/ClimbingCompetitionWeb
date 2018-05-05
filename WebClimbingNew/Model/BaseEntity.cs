namespace Climbing.Web.Model
{
    using System;
    using Climbing.Web.Utilities;

    public abstract class BaseEntity : IIdentityObject
    {
        [SerializeSkip]
        public Guid Id { get; private set; }

        public DateTimeOffset WhenCreated { get; private set; }

        public DateTimeOffset WhenChanged { get; private set; }
    }
}
