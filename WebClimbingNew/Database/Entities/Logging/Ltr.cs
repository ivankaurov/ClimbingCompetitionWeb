using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Database.Services;
using Utilities;

namespace Database.Entities.Logging
{
    [Table("ltr")]
    public class Ltr<T> : BaseEntity<T>
    {
        public Ltr(IIdentityProvider<T> identityProvider)
        : base(identityProvider)
        {
            this.Objects = new List<LtrObject<T>>();
        }

        protected Ltr()
        {
        }

        public virtual ICollection<LtrObject<T>> Objects { get; set; }

        public void AddNewObject(IIdentityObject<T> obj, IIdentityProvider<T> identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject<T>(obj, identityProvider)
            {
                ChangeType = ChangeType.New,
            };

            ltrObject.SetNewValues(obj, identityProvider);
            this.Objects.Add(ltrObject);
        }

        public void AddObjectBeforeChange(IIdentityObject<T> obj, IIdentityProvider<T> identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject<T>(obj, identityProvider)
            {
                ChangeType = ChangeType.Update,
            };

            ltrObject.SetOldValues(obj, identityProvider);
            this.Objects.Add(ltrObject);
        }

        public void AddObjectAfterChange(IIdentityObject<T> obj, IIdentityProvider<T> identityProvider)
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

        public void AddDeletedObject(IIdentityObject<T> obj, IIdentityProvider<T> identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));

            var ltrObject = this.Objects.FirstOrDefault(o => o.ObjectId.Equals(obj.Id));
            if(ltrObject != null)
            {
                throw new ArgumentException($"Object Id={obj.Id} already added. ChangeType={ltrObject.ChangeType}", nameof(obj));
            }

            ltrObject = new LtrObject<T>(obj, identityProvider)
            {
                ChangeType = ChangeType.Delete,
            };

            ltrObject.SetOldValues(obj, identityProvider);
            this.Objects.Add(ltrObject);
        }
    }
}