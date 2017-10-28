using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Database.Entities.Logging
{
    public class Ltr : BaseEntity
    {
        public virtual ICollection<LtrObject> Objects { get; set; } = new List<LtrObject>();

        public void AddNewObject(IIdentityObject obj)
        {
            Guard.NotNull(obj, nameof(obj));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj)
            {
                ChangeType = ChangeType.New,
            };

            ltrObject.SetNewValues(obj);
            this.Objects.Add(ltrObject);
        }

        public void AddObjectBeforeChange(IIdentityObject obj)
        {
            Guard.NotNull(obj, nameof(obj));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj)
            {
                ChangeType = ChangeType.Update,
            };

            ltrObject.SetOldValues(obj);
            this.Objects.Add(ltrObject);
        }

        public void AddObjectAfterChange(IIdentityObject obj)
        {
            Guard.NotNull(obj, nameof(obj));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject == null)
            {
                throw new ArgumentException($"Object Id={obj.Id} not found.", nameof(obj));
            }

            ltrObject.SetNewValues(obj);
        }

        public void AddDeletedObject(IIdentityObject obj)
        {
            Guard.NotNull(obj, nameof(obj));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj)
            {
                ChangeType = ChangeType.Delete,
            };

            ltrObject.SetOldValues(obj);
            this.Objects.Add(ltrObject);
        }
    }
}