using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Entities.Logging
{
    [Table("ltr_object_properties")]
    public class LtrObjectProperties<T> : BaseEntity<T>
    {
        public LtrObjectProperties(IIdentityProvider<T> identityProvider) : base(identityProvider)
        {
        }

        protected LtrObjectProperties()
        {
        }

        public T LtrObjectId { get; set; }
        public virtual LtrObject<T> LtrObject { get; set; }

        [Required]
        [MaxLength(512)]
        public string PropertyName { get; set; }

        [Required]
        [MaxLength(2048)]
        public string PropertyType { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
