using Climbing.Web.Model.Autonumeration;
using Climbing.Web.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Climbing.Web.Database.EntityBuilderExtensions
{
    internal static class AutonumModelBuilderExtensions
    {
        public static ModelBuilder MapAutonumDescriptionEntity(this ModelBuilder modelBuilder)
        {
            Guard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.BuildBaseEntityColumns<AutoNumDescription>("autonum_descriptions");

            modelBuilder.Entity<AutoNumDescription>().BuildStringProperty(p => p.Format, 255, columnName: "format_string");
            modelBuilder.Entity<AutoNumDescription>().Property(p => p.IncludeParentNumber).HasColumnName("include_parent_number");
            modelBuilder.Entity<AutoNumDescription>().BuildStringProperty(p => p.Name, 64, columnName: "sequence_name", nullable: false);
            modelBuilder.Entity<AutoNumDescription>().Property(p => p.SplitByParent).HasColumnName("split_sequence_by_parent");

            modelBuilder.Entity<AutoNumDescription>().HasIndex(p => p.Name).IsUnique();

            return modelBuilder;
        }

        public static ModelBuilder MapAutonumValueEntity(this ModelBuilder modelBuilder)
        {
            Guard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.BuildBaseEntityColumns<AutoNumValue>("autonum_values");

            modelBuilder.Entity<AutoNumValue>().Property(p => p.Counter).HasColumnName("value");
            modelBuilder.Entity<AutoNumValue>().Property(p => p.DescriptionId).HasColumnName("autonum_description_id");
            modelBuilder.Entity<AutoNumValue>().Property(p => p.ParentObjectId).HasColumnName("split_by");

            modelBuilder.Entity<AutoNumValue>().HasIndex(p => new { p.ParentObjectId, p.DescriptionId }).IsUnique(false);
            modelBuilder.Entity<AutoNumValue>()
                .HasOne(p => p.AutoNumDescription)
                .WithMany(d => d.Values)
                .HasForeignKey(p => p.DescriptionId)
                .IsRequired();

            return modelBuilder;
        }
    }
}