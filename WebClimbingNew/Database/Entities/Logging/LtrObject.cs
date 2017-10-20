using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities;
using System.Linq;

namespace Database.Entities.Logging
{
    [Table("ltr_object")]
    public class LtrObject<T> : BaseEntity<T>
    {
        internal LtrObject(IIdentityObject<T> obj, IIdentityProvider<T> identityProvider) : base(identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            this.ObjectId = obj.Id;
            this.LogObjectClass = obj.GetType().FullName;
            this.Properties = new List<LtrObjectProperties<T>>();
        }

        protected LtrObject()
        {
        }

        [Required]
        public T ObjectId { get; protected set; }

        [Required]
        [MaxLength(2048)]
        public string LogObjectClass { get; protected set; }
        
        [NotMapped]
        public ChangeType ChangeType
        {
            get => Enum.TryParse(this.ChangeTypeString, true, out ChangeType result) ? result : default(ChangeType);
            internal set => this.ChangeTypeString = value.ToString();
        }

        public virtual ICollection<LtrObjectProperties<T>> Properties { get; set; }

        public T LtrId{ get; set; }

        public virtual Ltr<T> Ltr { get; set; }

        public LtrObjectProperties<T> this[string propertyName]
        {
            get
            {
                Guard.NotNullOrWhitespace(propertyName, nameof(propertyName));
                return this.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.Ordinal));
            }
        }

        protected string ChangeTypeString { get; set; }

        internal void SetNewValues(object obj, IIdentityProvider<T> identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));
            this.SetValues(obj, identityProvider, (p, v) => p.NewValue = v);
        }

        internal void SetOldValues(object obj, IIdentityProvider<T> identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));
            this.SetValues(obj, identityProvider, (p, v) => p.OldValue = v);
        }

        private void SetValues(object obj, IIdentityProvider<T> identityProvider, Action<LtrObjectProperties<T>, string> updateAction)
        {
            var values = ObjectSerializer.ExtractProperties(obj);
            foreach(var v in values)
            {
                var item = this.GetOrAddObjectProperty(v.Key, v.Value.Type, identityProvider);
                updateAction(item, v.Value.Value?.ToString());
            }
        }

        private LtrObjectProperties<T> GetOrAddObjectProperty(string propertyName, Type propertyType, IIdentityProvider<T> identityProvider)
        {
            var existingItem = this.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.Ordinal) && p.PropertyType.Equals(propertyType.FullName, StringComparison.Ordinal));
            if(existingItem == null)
            {
                existingItem = new LtrObjectProperties<T>(identityProvider)
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
