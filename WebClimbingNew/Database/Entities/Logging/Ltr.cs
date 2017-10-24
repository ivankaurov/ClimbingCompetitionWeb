using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Database.Services;
using Utilities;

namespace Database.Entities.Logging
{
    public class Ltr : BaseEntity
    {
        public Ltr(IIdentityProvider identityProvider)
        : base(identityProvider)
        {
            this.Objects = new List<LtrObject>();
        }

        protected Ltr()
        {
        }

        public virtual ICollection<LtrObject> Objects { get; set; }

        public void AddNewObject(IIdentityObject obj, IIdentityProvider identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj, identityProvider)
            {
                ChangeType = ChangeType.New,
            };

            ltrObject.SetNewValues(obj, identityProvider);
            this.Objects.Add(ltrObject);
        }

        public void AddObjectBeforeChange(IIdentityObject obj, IIdentityProvider identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj, identityProvider)
            {
                ChangeType = ChangeType.Update,
            };

            ltrObject.SetOldValues(obj, identityProvider);
            this.Objects.Add(ltrObject);
        }

        public void AddObjectAfterChange(IIdentityObject obj, IIdentityProvider identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject == null)
            {
                throw new ArgumentException($"Object Id={obj.Id} not found.", nameof(obj));
            }

            ltrObject.SetNewValues(obj, identityProvider);
        }

        public void AddDeletedObject(IIdentityObject obj, IIdentityProvider identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject(obj, identityProvider)
            {
                ChangeType = ChangeType.Delete,
            };

            ltrObject.SetOldValues(obj, identityProvider);
            this.Objects.Add(ltrObject);
        }
    }
}