using System.Collections.Generic;
using Climbing.Web.Utilities;

namespace Climbing.Web.Model.Autonumeration
{
    public class AutoNumDescription : BaseEntity
    {
        public AutoNumDescription(string name, bool splitByParent, bool includeParentNumber, string format)
        {
            Guard.NotNullOrWhitespace(name, nameof(name));

            this.Name = name;
            this.SplitByParent = splitByParent;
            this.IncludeParentNumber = includeParentNumber;
            this.Format = format;
        }

        // To be used by entity framework
        private AutoNumDescription()
        {
        }

        public string Name{ get; private set; }

        public bool SplitByParent { get; private set; }

        public bool IncludeParentNumber { get; private set; }

        public string Format { get; private set; }

        public ICollection<AutoNumValue> Values { get; private set; } = new List<AutoNumValue>();
    }
}