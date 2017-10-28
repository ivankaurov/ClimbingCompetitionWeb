using System;
using System.Linq.Expressions;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utilities;

namespace Database
{
    internal static class EntityTypeBuilderExtensions
    {
        public static void BuildKey<T>(this EntityTypeBuilder<T> builder, string tableName)
            where T : class, IIdentityObject
        {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNullOrWhitespace(tableName, nameof(tableName));

            builder.ToTable(tableName)
                .HasKey(e => e.Id);
            builder.BuildStringProperty(e => e.Id, 64, false, "id", false)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<IdentityProvider>();
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
            if(!string.IsNullOrWhiteSpace(columnName))
            {
                result = result.HasColumnName(columnName);
            }

            if(maxLength > 0)
            {
                result = result.HasMaxLength(maxLength.Value);
            }

            return result;
        }
    }
}