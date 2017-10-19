using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities;

namespace Database.Entities
{
    [Table("entities")]
    public abstract class BaseEntity<T> : IIdentityObject<T>
    {
        protected BaseEntity(IIdentityProvider<T> identityProvider)
        {
            this.Id = identityProvider.CreateNewIdentity();
            this.ObjectClass = this.GetType().FullName;
            this.WhenCreated = this.WhenChanged = DateTimeOffset.Now;
        }

        protected BaseEntity()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [SerializeSkip]
        public T Id { get; private set; }

        [MaxLength(2048)]
        [SerializeSkip]
        public string ObjectClass { get; private set; }

        public DateTimeOffset WhenCreated { get; private set; }
        public DateTimeOffset WhenChanged { get; private set; }

        public void Touch()
        {
            this.WhenChanged = DateTimeOffset.Now;
        }
    }
}
