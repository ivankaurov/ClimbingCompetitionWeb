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
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}