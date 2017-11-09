using Microsoft.EntityFrameworkCore;

using Climbing.Web.Model;
using Climbing.Web.Model.Logging;
using Climbing.Web.Database.EntityBuilderExtensions;
using System;
using System.Linq;
using System.Threading;

namespace Climbing.Web.Database
{
    public class ClimbingContext : DbContext
    {
        internal const string RootTeamCode = "";

        private readonly Lazy<Team> rootTeam;

        public ClimbingContext(DbContextOptions<ClimbingContext> options) : base(options)
        {
            this.rootTeam = new Lazy<Team>(
                () => this.Teams.SingleAsync(t => t.ParentId == null && t.Code == RootTeamCode).GetAwaiter().GetResult(),
                LazyThreadSafetyMode.PublicationOnly);
        }

        public DbSet<Ltr> LogicTransactions { get; set; }

        public DbSet<LtrObject> LtrObjects { get; set; }
        public DbSet<LtrObjectProperties> LtrObjectProperties { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Person> People { get; set; }

        public Team RootTeam => this.rootTeam.Value;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapLtr()
                        .MapLtrObject()
                        .MapLtrObjectProperties()
                        .MapPersonEntity()
                        .MapTeamEntity();
            base.OnModelCreating(modelBuilder);
        }
    }

}
