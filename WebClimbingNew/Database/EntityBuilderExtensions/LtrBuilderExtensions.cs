using Climbing.Web.Model.Logging;
using Microsoft.EntityFrameworkCore;

namespace Climbing.Web.Database.EntityBuilderExtensions
{
    internal static class LtrBuilderExtensions
    {
        public static ModelBuilder MapLtr(this ModelBuilder modelBuilder)
        {
            modelBuilder.BuildBaseEntityColumns<Ltr>("ltr");
            modelBuilder.Entity<Ltr>()
                .HasMany(l => l.Objects)
                .WithOne(o => o.Ltr)
                .HasForeignKey(o => o.LtrId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            return modelBuilder;
        }

        public static ModelBuilder MapLtrObject(this ModelBuilder modelBuilder)
        {
            modelBuilder.BuildBaseEntityColumns<LtrObject>("ltr_objects");
            modelBuilder.Entity<LtrObject>().Ignore(p => p.ChangeType);
            modelBuilder.Entity<LtrObject>().BuildStringProperty(e => e.ChangeTypeString, 16, false, "ChangeType", false);
            modelBuilder.Entity<LtrObject>().BuildStringProperty(e => e.LogObjectClass, 255, false, nullable: false);
            modelBuilder.Entity<LtrObject>()
                .HasMany(o => o.Properties)
                .WithOne(p => p.LtrObject)
                .HasForeignKey(p => p.LtrObjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            return modelBuilder;
        }

        public static ModelBuilder MapLtrObjectProperties(this ModelBuilder modelBuilder)
        {
            modelBuilder.BuildBaseEntityColumns<LtrObjectProperties>("ltr_object_properties");
            modelBuilder.Entity<LtrObjectProperties>().BuildStringProperty(e => e.PropertyName, 255, false, nullable: false);
            modelBuilder.Entity<LtrObjectProperties>().BuildStringProperty(e => e.PropertyType, 255, false, nullable: false);

            return modelBuilder;
        }
    }
}