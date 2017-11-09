using System;

using Climbing.Web.Utilities;

namespace Climbing.Web.Model
{
    public abstract class BaseEntity : IIdentityObject
    {
        private DateTimeOffset whenCreated;

        [SerializeSkip]
        public Guid Id { get; private set; }

        public DateTimeOffset WhenCreated
        {
            get => this.whenCreated;
            set
            {
                this.whenCreated = value;
                if(this.whenCreated == default(DateTimeOffset) || this.whenCreated > this.WhenChanged)
                {
                    this.WhenChanged = this.whenCreated;
                }
            }
        }
        public DateTimeOffset WhenChanged { get; private set; }

        public void Touch()
        {
            this.WhenChanged = DateTimeOffset.Now;
        }
    }
}
