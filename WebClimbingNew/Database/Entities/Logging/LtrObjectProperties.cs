using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities;

namespace Database.Entities.Logging
{
    public class LtrObjectProperties : BaseEntity
    {
        internal LtrObjectProperties(IIdentityProvider identityProvider) : base(identityProvider)
        {
        }

        protected LtrObjectProperties()
        {
        }

        public string LtrObjectId { get; set; }
        public virtual LtrObject LtrObject { get; set; }
        
        public string PropertyName { get; set; }
        
        public string PropertyType { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
