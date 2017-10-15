using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Entities
{
    [Table("entities")]
    public abstract class BaseEntity<T> : IIdentityObject<T>
    {
        protected BaseEntity(IIdentityProvider<T> identityProvider)
        {
            this.Id = identityProvider.CreateNewIdentity();
            this.ObjectClass = this.GetType().Name;
            this.WhenCreated = this.WhenChanged = DateTimeOffset.Now;
        }

        protected BaseEntity()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public T Id { get; private set; }

        [MaxLength(2048)]
        public string ObjectClass { get; private set; }

        public DateTimeOffset WhenCreated { get; private set; }
        public DateTimeOffset WhenChanged { get; private set; }

        public void Touch()
        {
            this.WhenChanged = DateTimeOffset.Now;
        }
    }
}
