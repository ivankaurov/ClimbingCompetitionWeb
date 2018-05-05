namespace Climbing.Web.Database
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Database.EntityBuilderExtensions;
    using Climbing.Web.Model;
    using Climbing.Web.Model.Logging;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore;

    public class ClimbingContext : DbContext, IUnitOfWork
    {
        public ClimbingContext(DbContextOptions<ClimbingContext> options)
            : base(options)
        {
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ltr = new Ltr();
            this.ChangeTracker.DetectChanges();

            this.SetupNewObjects(ltr);
            this.SetupChangedObjects(ltr);
            this.SetupDeletedObjects(ltr);

            await this.Repository<Ltr>().AddAsync(ltr, cancellationToken);
            this.ChangeTracker.DetectChanges();

            this.ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            this.ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public DbSet<T> Repository<T>()
            where T : class => this.Set<T>();

        public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var contextTransaction = await this.Database.BeginTransactionAsync(cancellationToken);
            return new ContextTransaction(contextTransaction);
        }

        public ITransaction BeginTransaction()
        {
            var innerTransaction = this.Database.BeginTransaction();
            return new ContextTransaction(innerTransaction);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapLtr()
                        .MapLtrObject()
                        .MapLtrObjectProperties()
                        .MapPersonEntity()
                        .MapTeamEntity()
                        .MapAutonumDescriptionEntity()
                        .MapAutonumValueEntity();
            base.OnModelCreating(modelBuilder);
        }

        private void SetupNewObjects(Ltr ltr)
        {
            var tms = DateTimeOffset.UtcNow;
            foreach (var entry in this.ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Added)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                entry.SetProperty(e => e.WhenChanged, tms);
                entry.SetProperty(e => e.WhenCreated, tms);
                if (entry.ShouldSerialize())
                {
                    ltr.AddObject(entry, ChangeType.New);
                }
            }
        }

        private void SetupChangedObjects(Ltr ltr)
        {
            var tms = DateTimeOffset.UtcNow;
            foreach (var entry in this.ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Modified)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                entry.SetProperty(e => e.WhenChanged, tms);
                if (entry.ShouldSerialize())
                {
                    ltr.AddObject(entry, ChangeType.Update);
                }
            }
        }

        private void SetupDeletedObjects(Ltr ltr)
        {
            var tms = DateTimeOffset.UtcNow;
            foreach (var entry in this.ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Deleted)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                if (entry.ShouldSerialize())
                {
                    ltr.AddObject(entry, ChangeType.Delete);
                }
            }
        }
    }
}
