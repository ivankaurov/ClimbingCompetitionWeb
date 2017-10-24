﻿using Database.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities;
using System.Linq;

namespace Database.Entities.Logging
{
    public class LtrObject : BaseEntity
    {
        internal LtrObject(IIdentityObject obj, IIdentityProvider identityProvider) : base(identityProvider)
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

        public virtual ICollection<LtrObjectProperties> Properties { get; set; }

        public Guid LtrId{ get; set; }

        public virtual Ltr Ltr { get; set; }

        public LtrObjectProperties this[string propertyName]
        {
            get
            {
                Guard.NotNullOrWhitespace(propertyName, nameof(propertyName));
                return this.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.Ordinal));
            }
        }

        public string ChangeTypeString { get; private set; }

        internal void SetNewValues(object obj, IIdentityProvider identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));
            this.SetValues(obj, identityProvider, (p, v) => p.NewValue = v);
        }

        internal void SetOldValues(object obj, IIdentityProvider identityProvider)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(identityProvider, nameof(identityProvider));
            this.SetValues(obj, identityProvider, (p, v) => p.OldValue = v);
        }

        private void SetValues(object obj, IIdentityProvider identityProvider, Action<LtrObjectProperties, string> updateAction)
        {
            var values = ObjectSerializer.ExtractProperties(obj);
            foreach(var v in values)
            {
                var item = this.GetOrAddObjectProperty(v.Key, v.Value.Type, identityProvider);
                updateAction(item, v.Value.Value?.ToString());
            }
        }

        private LtrObjectProperties GetOrAddObjectProperty(string propertyName, Type propertyType, IIdentityProvider identityProvider)
        {
            var existingItem = this.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.Ordinal) && p.PropertyType.Equals(propertyType.FullName, StringComparison.Ordinal));
            if(existingItem == null)
            {
                existingItem = new LtrObjectProperties(identityProvider)
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
