using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities;

namespace Database.Entities.Logging
{
    [Table("ltr_object")]
    public class LtrObject<T> : BaseEntity<T>
    {
        public LtrObject(IIdentityObject<T> obj, IIdentityProvider<T> identityProvider) : base(identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            this.ObjectId = obj.Id;
            this.LogObjectClass = obj.GetType().FullName;
            this.Properties = new List<LtrObjectProperties<T>>();
        }

        protected LtrObject()
        {
        }

        [Required]
        public T ObjectId { get; protected set; }

        [Required]
        [MaxLength(2048)]
        public string LogObjectClass { get; protected set; }
        
        [NotMapped]
        public ChangeType ChangeType
        {
            get => Enum.TryParse(this.ChangeTypeString, true, out ChangeType result) ? result : default(ChangeType);
            set => this.ChangeTypeString = value.ToString();
        }

        public virtual ICollection<LtrObjectProperties<T>> Properties { get; set; }

        public T LtrId{ get; set; }

        public virtual Ltr<T> Ltr { get; set; }

        protected string ChangeTypeString { get; set; }
    }
}
