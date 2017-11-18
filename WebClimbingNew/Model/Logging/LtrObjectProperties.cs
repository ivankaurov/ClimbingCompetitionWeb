using System;
using Climbing.Web.Utilities;

namespace Climbing.Web.Model.Logging
{
    [SerializeSkip]
    public class LtrObjectProperties : BaseEntity
    {
        internal LtrObjectProperties()
        {
        }

        public Guid LtrObjectId { get; set; }
        public LtrObject LtrObject { get; set; }
        
        public string PropertyName { get; set; }
        
        public string PropertyType { get; set; }

        public string Value { get; set; }
    }
}
