using System;
using System.Collections.Generic;

namespace Climbing.Web.Model
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public Guid? ParentId { get; set; }

        public Team Parent { get; set; }

        public ICollection<Team> Children { get; set; }
    }
}