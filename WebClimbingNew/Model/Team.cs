namespace Climbing.Web.Model
{
    using System;
    using System.Collections.Generic;

    public class Team : BaseEntity
    {
        public const string RootTeamCode = "";

        public const string RootTeamName = "Россия";

        public string Name { get; set; }

        public string Code { get; set; }

        public Guid? ParentId { get; set; }

        public Team Parent { get; set; }

        public ICollection<Team> Children { get; set; } = new List<Team>();
    }
}