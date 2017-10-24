﻿// <auto-generated />
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Database.Migrations
{
    [DbContext(typeof(ClimbingContext))]
    [Migration("20171024202428_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Database.Entities.AccountEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<string>("Password")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("Database.Entities.Logging.Ltr", b =>
                {
                    b.Property<Guid>("Id");

                    b.HasKey("Id");

                    b.ToTable("ltr");
                });

            modelBuilder.Entity("Database.Entities.Logging.LtrObject", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("ChangeTypeString")
                        .IsRequired()
                        .HasColumnName("ChangeType")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<string>("LogObjectClass")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<Guid>("LtrId");

                    b.Property<Guid>("ObjectId");

                    b.HasKey("Id");

                    b.HasIndex("LtrId");

                    b.ToTable("ltr_objects");
                });

            modelBuilder.Entity("Database.Entities.Logging.LtrObjectProperties", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("LtrObjectId");

                    b.Property<string>("NewValue");

                    b.Property<string>("OldValue");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("PropertyType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("LtrObjectId");

                    b.ToTable("ltr_object_properties");
                });

            modelBuilder.Entity("Database.Entities.Logging.LtrObject", b =>
                {
                    b.HasOne("Database.Entities.Logging.Ltr", "Ltr")
                        .WithMany("Objects")
                        .HasForeignKey("LtrId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.Logging.LtrObjectProperties", b =>
                {
                    b.HasOne("Database.Entities.Logging.LtrObject", "LtrObject")
                        .WithMany("Properties")
                        .HasForeignKey("LtrObjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
