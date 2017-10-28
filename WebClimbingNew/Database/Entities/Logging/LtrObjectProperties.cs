namespace Database.Entities.Logging
{
    public class LtrObjectProperties : BaseEntity
    {
        internal LtrObjectProperties()
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
