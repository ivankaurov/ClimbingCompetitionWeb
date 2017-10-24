using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities;

namespace Database.Entities
{
    public abstract class BaseEntity : IIdentityObject
    {
        protected BaseEntity(IIdentityProvider identityProvider)
        {
            this.Id = identityProvider.CreateNewIdentity();
            this.WhenCreated = this.WhenChanged = DateTimeOffset.Now;
        }

        protected BaseEntity()
        {
        }

        [SerializeSkip]
        public Guid Id { get; private set; }

        public DateTimeOffset WhenCreated { get; private set; }
        public DateTimeOffset WhenChanged { get; private set; }

        public void Touch()
        {
            this.WhenChanged = DateTimeOffset.Now;
        }
    }
}
