﻿// <auto-generated />
using System;
using Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Identity.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    partial class IdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Data.Shared.Identity.Entities.UserCredentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Credentials");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(5753),
                            CreatedBy = "System",
                            ExpiresAt = new DateTime(2025, 3, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(5753),
                            Password = "U3VwZXJTZWNyZXQ1MzRhYTQ1Yi05YWRkLTQzNGUtYWQzYy1mNzJlNWFkNTA1ZGE=",
                            Salt = "534aa45b-9add-434e-ad3c-f72e5ad505da",
                            UpdatedAt = new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(5753),
                            UpdatedBy = "System"
                        });
                });

            modelBuilder.Entity("Data.Shared.Identity.Entities.UserIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CredentialsId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CredentialsId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(6078),
                            CreatedBy = "System",
                            CredentialsId = 1,
                            Email = "admin.user@gmx.de",
                            FirstName = "Admin",
                            IsActive = true,
                            LastName = "User",
                            RoleId = 2,
                            UpdatedAt = new DateTime(2024, 12, 7, 15, 31, 27, 202, DateTimeKind.Local).AddTicks(6078),
                            UpdatedBy = "System"
                        });
                });

            modelBuilder.Entity("Data.Shared.Identity.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ResourceKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoleType")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Data.Shared.Identity.Entities.UserIdentity", b =>
                {
                    b.HasOne("Data.Shared.Identity.Entities.UserCredentials", "UserCredentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Shared.Identity.Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserCredentials");

                    b.Navigation("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}
