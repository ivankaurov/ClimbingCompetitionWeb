namespace Climbing.Web.Model.Autonumeration
{
    using System;
    using Climbing.Web.Utilities;

    public class AutoNumValue : BaseEntity
    {
        public AutoNumValue(long counter, AutoNumDescription autoNumDescription, Guid? parentObjectId)
        {
            Guard.NotNull(autoNumDescription, nameof(autoNumDescription));
            this.Counter = counter;
            this.AutoNumDescription = autoNumDescription;
            this.DescriptionId = autoNumDescription.Id;
            this.ParentObjectId = parentObjectId;
        }

        // To be used by EF
        private AutoNumValue()
        {
        }

        public long Counter { get; private set; }

        public Guid? ParentObjectId { get; private set; }

        public Guid DescriptionId { get; private set; }

        public AutoNumDescription AutoNumDescription { get; private set; }
    }
}