using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Entities.Logging
{
    [Table("ltr_object")]
    public class LtrObject<T> : BaseEntity<T>
    {
        public LtrObject(IIdentityProvider<T> identityProvider) : base(identityProvider)
        {
            this.Properties = new List<LtrObjectProperties<T>>();
        }

        protected LtrObject()
        {
        }

        [Required]
        public T ObjectId { get; set; }

        [Required]
        [MaxLength(2048)]
        public string LogObjectClass { get; set; }
        
        [NotMapped]
        public ChangeType ChangeType
        {
            get => Enum.TryParse(this.ChangeTypeString, true, out ChangeType result) ? result : default(ChangeType);
            set => this.ChangeTypeString = value.ToString();
        }

        public virtual ICollection<LtrObjectProperties<T>> Properties { get; set; }

        protected string ChangeTypeString { get; set; }
    }
}
