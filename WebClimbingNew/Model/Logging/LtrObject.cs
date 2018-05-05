namespace Climbing.Web.Model.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Climbing.Web.Utilities;

    [SerializeSkip]
    public class LtrObject : BaseEntity
    {
        internal LtrObject(IIdentityObject obj)
        {
            Guard.NotNull(obj, nameof(obj));
            this.ObjectId = obj.Id;
            this.LogObjectClass = obj.GetType().FullName;
            this.Properties = new List<LtrObjectProperties>();
        }

        protected LtrObject()
        {
        }

        public Guid ObjectId { get; protected set; }

        public string LogObjectClass { get; protected set; }

        public ChangeType ChangeType
        {
            get => Enum.TryParse(this.ChangeTypeString, true, out ChangeType result) ? result : default(ChangeType);
            internal set => this.ChangeTypeString = value.ToString();
        }

        public ICollection<LtrObjectProperties> Properties { get; set; }

        public Guid LtrId { get; set; }

        public Ltr Ltr { get; set; }

        public string ChangeTypeString { get; private set; }

        public LtrObjectProperties this[string propertyName]
        {
            get
            {
                Guard.NotNullOrWhitespace(propertyName, nameof(propertyName));
                return this.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.Ordinal));
            }
        }

        internal void SetValues(object obj)
        {
            Guard.NotNull(obj, nameof(obj));
            var values = ObjectSerializer.ExtractProperties(obj);
            foreach (var v in values)
            {
                var item = this.GetOrAddObjectProperty(v.Key, v.Value.Type);
                item.Value = v.Value.Value?.ToString();
            }
        }

        private LtrObjectProperties GetOrAddObjectProperty(string propertyName, Type propertyType)
        {
            var existingItem = this.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.Ordinal) && p.PropertyType.Equals(propertyType.FullName, StringComparison.Ordinal));
            if (existingItem == null)
            {
                existingItem = new LtrObjectProperties
                {
                    PropertyName = propertyName,
                    PropertyType = propertyType.FullName,
                    LtrObject = this,
                    LtrObjectId = this.Id,
                };

                this.Properties.Add(existingItem);
            }

            return existingItem;
        }
    }
}
