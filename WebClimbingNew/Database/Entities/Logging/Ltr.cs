using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        }
    }
}