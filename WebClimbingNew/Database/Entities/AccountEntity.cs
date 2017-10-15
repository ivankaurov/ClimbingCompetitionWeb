using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Entities
{
    public class AccountEntity<T>: BaseEntity<T>
    {
        public AccountEntity(IIdentityProvider<T> identityProvider) : base(identityProvider)
        {
        }

        protected AccountEntity()
        {
        }

        [Required]
        [MaxLength(256)]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(4096)]
        public string Password { get; set; }
    }
}
