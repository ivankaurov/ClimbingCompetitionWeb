using Microsoft.EntityFrameworkCore;

using Climbing.Web.Model;
using Climbing.Web.Model.Logging;
using Climbing.Web.Database.EntityBuilderExtensions;
using Climbing.Web.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();

            foreach(var entry in ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                entry.SetProperty(e => e.WhenChanged, DateTimeOffset.UtcNow);
            }

            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();

            foreach(var entry in ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                entry.SetProperty(e => e.WhenChanged, DateTimeOffset.UtcNow);
            }

            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }
    }

}
