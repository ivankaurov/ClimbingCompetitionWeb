namespace Climbing.Web.Model.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Climbing.Web.Utilities;

    [SerializeSkip]
    public class Ltr : BaseEntity
    {
        public ICollection<LtrObject> Objects { get; set; } = new List<LtrObject>();

        public void AddObject(IIdentityObject obj, ChangeType changeType)
        {
            Guard.NotNull(obj, nameof(obj));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if (ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj)
            {
                ChangeType = changeType,
            };

            ltrObject.SetValues(obj);
            this.Objects.Add(ltrObject);
        }
    }
}