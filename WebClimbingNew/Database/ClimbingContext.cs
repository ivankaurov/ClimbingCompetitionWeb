using Database.Entities;
using Database.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database
{
    public class ClimbingContext : DbContext
    {
        public ClimbingContext(DbContextOptions<ClimbingContext> options) : base(options)
        {
        }

        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<Ltr> LogicTransactions { get; set; }

        public DbSet<LtrObject> LtrObjects { get; set; }
        public DbSet<LtrObjectProperties> LtrObjectProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>().BuildKey("accounts");
            modelBuilder.Entity<AccountEntity>()
                        .Property(a => a.EmailAddress)
                        .IsRequired()
                        .IsUnicode()
                        .HasMaxLength(255);
            modelBuilder.Entity<AccountEntity>()
                        .Property(a => a.Password)
                        .IsUnicode();

            modelBuilder.Entity<Ltr>().BuildKey("ltr");
            modelBuilder.Entity<Ltr>()
                .HasMany(l => l.Objects)
                .WithOne(o => o.Ltr)
                .HasForeignKey(o => o.LtrId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<LtrObject>().BuildKey("ltr_objects");
            modelBuilder.Entity<LtrObject>().Ignore(p => p.ChangeType);
            modelBuilder.Entity<LtrObject>()
                .Property(p => p.ChangeTypeString)
                .HasColumnName("ChangeType")
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);
            modelBuilder.Entity<LtrObject>()
                .Property(p => p.LogObjectClass)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();
            modelBuilder.Entity<LtrObject>()
                .HasMany(o => o.Properties)
                .WithOne(p => p.LtrObject)
                .HasForeignKey(p => p.LtrObjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LtrObjectProperties>().BuildKey("ltr_object_properties");
            modelBuilder.Entity<LtrObjectProperties>()
                .Property(p => p.PropertyName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);
            modelBuilder.Entity<LtrObjectProperties>()
                .Property(p => p.PropertyType)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            base.OnModelCreating(modelBuilder);
        }
    }

}
