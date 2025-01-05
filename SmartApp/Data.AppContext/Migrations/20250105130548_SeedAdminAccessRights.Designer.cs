﻿// <auto-generated />
using System;
using Data.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.AppContext.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250105130548_SeedAdminAccessRights")]
    partial class SeedAdminAccessRights
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Data.Shared.AccessRights.AccessRightEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AccessRights");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4610),
                            CreatedBy = "System",
                            Name = "Administration"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4613),
                            CreatedBy = "System",
                            Name = "UserAdministration"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4614),
                            CreatedBy = "System",
                            Name = "Settings"
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4615),
                            CreatedBy = "System",
                            Name = "EmailAccountSettings"
                        });
                });

            modelBuilder.Entity("Data.Shared.AccessRights.UserAccessRightEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessRightId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Deny")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Edit")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("View")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("AccessRightId");

                    b.ToTable("UserAccessRights");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessRightId = 1,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4740),
                            CreatedBy = "System",
                            Deny = false,
                            Edit = true,
                            UserId = 1,
                            View = true
                        },
                        new
                        {
                            Id = 2,
                            AccessRightId = 2,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4742),
                            CreatedBy = "System",
                            Deny = false,
                            Edit = true,
                            UserId = 1,
                            View = true
                        },
                        new
                        {
                            Id = 3,
                            AccessRightId = 3,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4743),
                            CreatedBy = "System",
                            Deny = false,
                            Edit = true,
                            UserId = 1,
                            View = true
                        },
                        new
                        {
                            Id = 4,
                            AccessRightId = 4,
                            CreatedAt = new DateTime(2025, 1, 5, 13, 5, 48, 219, DateTimeKind.Utc).AddTicks(4744),
                            CreatedBy = "System",
                            Deny = false,
                            Edit = true,
                            UserId = 1,
                            View = true
                        });
                });

            modelBuilder.Entity("Data.Shared.EmailAccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EncodedPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MessageLogJson")
                        .HasColumnType("longtext");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<int>("ProviderType")
                        .HasColumnType("int");

                    b.Property<string>("Server")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailAccounts");
                });

            modelBuilder.Entity("Data.Shared.Logging.LogMessageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("longtext");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<string>("Module")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LogMessages");
                });

            modelBuilder.Entity("Data.Shared.AccessRights.UserAccessRightEntity", b =>
                {
                    b.HasOne("Data.Shared.AccessRights.AccessRightEntity", "AccessRight")
                        .WithMany()
                        .HasForeignKey("AccessRightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccessRight");
                });
#pragma warning restore 612, 618
        }
    }
}
