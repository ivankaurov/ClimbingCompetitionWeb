using Microsoft.EntityFrameworkCore;

using Climbing.Web.Model;
using Climbing.Web.Model.Logging;
using Climbing.Web.Database.EntityBuilderExtensions;
using Climbing.Web.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service.Repository;

namespace Climbing.Web.Database
{
    public class ClimbingContext : DbContext, IUnitOfWork
    {
        public ClimbingContext(DbContextOptions<ClimbingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapLtr()
                        .MapLtrObject()
                        .MapLtrObjectProperties()
                        .MapPersonEntity()
                        .MapTeamEntity();
            base.OnModelCreating(modelBuilder);
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

        public DbSet<T> Repository<T>() where T : class => this.Set<T>();

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
    }

}
