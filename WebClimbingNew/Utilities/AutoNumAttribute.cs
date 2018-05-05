namespace Climbing.Web.Utilities
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AutoNumAttribute : Attribute
    {
        public AutoNumAttribute(string sequenceName = null, string formatString = null)
        {
            this.SequenceName = sequenceName;
            this.FormatString = string.IsNullOrWhiteSpace(formatString) ? null : formatString;
        }

        public AutoNumAttribute(string parentIdPropertyName, string sequenceName = null, string formatString = null)
            : this(sequenceName, formatString)
        {
            Guard.NotNullOrWhitespace(parentIdPropertyName, nameof(parentIdPropertyName));
            this.ParentIdPropertyName = parentIdPropertyName;
        }

        public AutoNumAttribute(string parentIdPropertyName, string parentNavigationalPropertyName, string parentNumberPropertyName, string sequenceName = null, string formatString = null)
            : this(parentIdPropertyName, sequenceName, formatString)
        {
            Guard.NotNullOrWhitespace(parentNavigationalPropertyName, nameof(parentNavigationalPropertyName));
            Guard.NotNullOrWhitespace(parentNumberPropertyName, nameof(parentNumberPropertyName));
            this.ParentNavigationalPropertyName = parentNavigationalPropertyName;
            this.ParentNumberPropertyName = parentNumberPropertyName;
        }

        public string SequenceName { get; }

        public string FormatString { get; }

        public string ParentIdPropertyName { get; }

        public string ParentNavigationalPropertyName { get; }

        public string ParentNumberPropertyName { get; }
    }
}