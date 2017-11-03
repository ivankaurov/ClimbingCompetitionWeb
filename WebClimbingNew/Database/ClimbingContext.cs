using Microsoft.EntityFrameworkCore;

using Climbing.Web.Model;
using Climbing.Web.Model.Logging;

namespace Climbing.Web.Database
{
    public class ClimbingContext : DbContext
    {
        public ClimbingContext(DbContextOptions<ClimbingContext> options) : base(options)
        {
        }

        public DbSet<Ltr> LogicTransactions { get; set; }

        public DbSet<LtrObject> LtrObjects { get; set; }
        public DbSet<LtrObjectProperties> LtrObjectProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ltr>().BuildKey("ltr");
            modelBuilder.Entity<Ltr>()
                .HasMany(l => l.Objects)
                .WithOne(o => o.Ltr)
                .HasForeignKey(o => o.LtrId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<LtrObject>().BuildKey("ltr_objects");
            modelBuilder.Entity<LtrObject>().Ignore(p => p.ChangeType);
            modelBuilder.Entity<LtrObject>().BuildStringProperty(e => e.ChangeTypeString, 16, false, "ChangeType", false);
            modelBuilder.Entity<LtrObject>().BuildStringProperty(e => e.LogObjectClass, 255, false, nullable: false);
            modelBuilder.Entity<LtrObject>()
                .HasMany(o => o.Properties)
                .WithOne(p => p.LtrObject)
                .HasForeignKey(p => p.LtrObjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LtrObjectProperties>().BuildKey("ltr_object_properties");
            modelBuilder.Entity<LtrObjectProperties>().BuildStringProperty(e => e.PropertyName, 255, false, nullable: false);
            modelBuilder.Entity<LtrObjectProperties>().BuildStringProperty(e => e.PropertyType, 255, false, nullable: false);

            base.OnModelCreating(modelBuilder);
        }
    }

}
