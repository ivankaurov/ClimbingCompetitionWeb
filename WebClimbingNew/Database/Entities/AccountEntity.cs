using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Entities
{
    public class AccountEntity : BaseEntity
    {
        public AccountEntity(IIdentityProvider identityProvider) : base(identityProvider)
        {
        }

        protected AccountEntity()
        {
        }

        public string EmailAddress { get; set; }
        
        public string Password { get; set; }
    }
}
