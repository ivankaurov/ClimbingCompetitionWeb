namespace Climbing.Web.Database.EntityBuilderExtensions
{
    using Climbing.Web.Model;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore;

    internal static class ImmutableModelBuilderExtensions
    {
        public static ModelBuilder MapPersonEntity(this ModelBuilder modelBuilder)
        {
            Guard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.BuildBaseEntityColumns<Person>("people")
                .BuildStringProperty(p => p.Surname, 255, columnName: "surname", nullable: false);

            modelBuilder.Entity<Person>().BuildStringProperty(p => p.Name, 255, columnName: "name", nullable: false);
            modelBuilder.Entity<Person>().Property(p => p.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
            modelBuilder.Entity<Person>().BuildStringProperty(p => p.Email, 255, columnName: "email");

            modelBuilder.Entity<Person>().HasIndex(p => new { p.Surname, p.Name, p.DateOfBirth }).IsUnique();

            return modelBuilder;
        }

        public static ModelBuilder MapTeamEntity(this ModelBuilder modelBuilder)
        {
            Guard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.BuildBaseEntityColumns<Team>("teams")
                .BuildStringProperty(t => t.Name, 255, columnName: "name", nullable: false);
            modelBuilder.Entity<Team>().BuildStringProperty(t => t.Code, 32, false, "code", false);
            modelBuilder.Entity<Team>().Property(t => t.ParentId).HasColumnName("parent_team_id").IsRequired(false);
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(t => t.ParentId)
                .IsRequired(false);

            modelBuilder.Entity<Team>().HasIndex(t => new { t.Name, t.ParentId }).IsUnique();
            modelBuilder.Entity<Team>().HasIndex(t => new { t.Code }).IsUnique();

            return modelBuilder;
        }
    }
}