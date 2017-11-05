using System;

using Climbing.Web.Utilities;

namespace Climbing.Web.Model
{
    public abstract class BaseEntity : IIdentityObject
    {
        protected BaseEntity()
        {
            this.WhenCreated = this.WhenChanged = DateTimeOffset.Now;
        }

        [SerializeSkip]
        public Guid Id { get; set; }

        public DateTimeOffset WhenCreated { get; private set; }
        public DateTimeOffset WhenChanged { get; private set; }

        public void Touch()
        {
            this.WhenChanged = DateTimeOffset.Now;
        }
    }
}
