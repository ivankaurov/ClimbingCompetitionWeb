using Database.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public abstract class ClimbingContextBase<T> : DbContext
    {
        protected ClimbingContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LtrObject<T>> LtrObjects { get; set; }
        public DbSet<LtrObjectProperties<T>> LtrObjectProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LtrObject<T>>()
                .HasMany(o => o.Properties)
                .WithOne(p => p.LtrObject)
                .HasForeignKey(p => p.LtrObjectId)
                .IsRequired();

            modelBuilder.Entity<Ltr<T>>()
                .HasMany(l => l.Objects)
                .WithOne(o => o.Ltr)
                .HasForeignKey(o => o.LtrId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ClimbingContext : ClimbingContextBase<Guid>
    {
        public ClimbingContext(DbContextOptions<ClimbingContext> options) : base(options)
        {
        }
    }

}
