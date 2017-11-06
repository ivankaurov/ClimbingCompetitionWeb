using System;

namespace Climbing.Web.Model.Logging
{
    public class LtrObjectProperties : BaseEntity
    {
        internal LtrObjectProperties()
        {
        }

        public Guid LtrObjectId { get; set; }
        public LtrObject LtrObject { get; set; }
        
        public string PropertyName { get; set; }
        
        public string PropertyType { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
