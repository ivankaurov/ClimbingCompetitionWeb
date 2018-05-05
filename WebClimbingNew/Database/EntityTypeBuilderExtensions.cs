namespace Climbing.Web.Database
{
    using System;
    using System.Linq.Expressions;
    using Climbing.Web.Model;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<T> BuildBaseEntityColumns<T>(this ModelBuilder modelBuilder, string tableName)
            where T : BaseEntity
        {
            Guard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<T>().BuildKey(tableName);
            modelBuilder.Entity<T>().Property(e => e.WhenCreated)
                .HasColumnName("when_created")
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasValueGenerator<TimeStampValueGenerator>();
            modelBuilder.Entity<T>().Property(e => e.WhenChanged)
                .IsRequired()
                .HasColumnName("when_changed");
            return modelBuilder.Entity<T>();
        }

        public static void BuildKey<T>(this EntityTypeBuilder<T> builder, string tableName)
            where T : class, IIdentityObject
        {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNullOrWhitespace(tableName, nameof(tableName));

            builder.ToTable(tableName)
                .HasKey(e => e.Id);
            builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        }

        public static PropertyBuilder<string> BuildStringProperty<T>(
            this EntityTypeBuilder<T> builder,
            Expression<Func<T, string>> property,
            int? maxLength = null,
            bool unicode = true,
            string columnName = null,
            bool nullable = true)
            where T : class
        {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(property, nameof(property));

            var result = builder.Property(property)
                .IsUnicode(unicode)
                .IsRequired(!nullable);
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                result = result.HasColumnName(columnName);
            }

            if (maxLength > 0)
            {
                result = result.HasMaxLength(maxLength.Value);
            }

            return result;
        }
    }
}