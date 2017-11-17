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
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace Climbing.Web.Database
{
    public class ClimbingContext : DbContext, IUnitOfWork
    {
        private static readonly ConcurrentDictionary<Type, Func<DbContext, object>> RepositoryGetDictionary
         = new ConcurrentDictionary<Type, Func<DbContext, object>>();

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

        public DbSet<T> Repository<T>() where T : class => (DbSet<T>)this.GetRepository(typeof(T));

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

        private void SetupNewObjects(Ltr ltr)
        {
            var tms = DateTimeOffset.UtcNow;
            foreach(var entry in ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Added)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                entry.SetProperty(e => e.WhenChanged, tms);
                entry.SetProperty(e => e.WhenCreated, tms);
                if(entry.ShouldSerialize())
                {
                    ltr.AddNewObject(entry);
                }
            }
        }

        private async Task SetupChangedObjects(Ltr ltr)
        {
            var tms = DateTimeOffset.UtcNow;
            foreach(var entry in ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Modified)
                            .Select(e => e.Entity)
                            .OfType<BaseEntity>())
            {
                entry.SetProperty(e => e.WhenChanged, tms);
                if(entry.ShouldSerialize())
                {
                    var oldValues = await this.FindEntity(entry.GetType(), entry.Id);
                    if(oldValues == null)
                    {
                        ltr.AddNewObject(entry);
                    }
                    else
                    {
                        ltr.AddObjectBeforeChange(oldValues);
                        ltr.AddObjectAfterChange(entry);
                    }
                }
            }
        }

        private object GetRepository(Type objectType)
          => RepositoryGetDictionary.GetOrAdd(objectType, key => CompileGetRepository(key)).Invoke(this);

        private static Func<DbContext, object> CompileGetRepository(Type objectType)
        {
            var dbcParam = Expression.Parameter(typeof(DbContext));
            var repositoryExpression = Expression.Call(dbcParam, nameof(DbContext.Set), new [] { objectType });
            var expr = Expression.Lambda<Func<DbContext, object>>(repositoryExpression, dbcParam);
            return expr.Compile();
        }

        private Task<BaseEntity> FindEntity(Type entityType, Guid id)
        {
            throw new NotImplementedException();
        }
    }

}
