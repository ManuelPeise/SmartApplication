﻿// <auto-generated />
using System;
using Data.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Databases.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Data.Shared.Email.EmailAccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("ConnectionTestPassed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ImapPort")
                        .HasColumnType("int");

                    b.Property<string>("ImapServer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ProviderType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailAccountTable");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailAddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EmailAddressTable");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailCleanerSettingsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailCleanerEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("EmailCleanerSettingsTable");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailCleanupConfigurationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("PredictedSpamIdentifierValue")
                        .HasColumnType("int");

                    b.Property<int?>("PredictedTargetFolderId")
                        .HasColumnType("int");

                    b.Property<int>("SpamIdentifierValue")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TargetFolderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AddressId");

                    b.HasIndex("PredictedTargetFolderId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TargetFolderId");

                    b.ToTable("EmailCleanupTable");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailSubjectEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EmailSubject")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EmailSubjectTable");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailTargetFolderEntity", b =>
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

                    b.Property<string>("TargetFolderName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EmailTargetFolderTable");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4314),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderUnknown",
                            TargetFolderName = "Unknown"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4318),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderFoodOrder",
                            TargetFolderName = "Food"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4318),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderTravel",
                            TargetFolderName = "Travel"
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4319),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderTax",
                            TargetFolderName = "Tax"
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4320),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderAccounts",
                            TargetFolderName = "Accounts"
                        },
                        new
                        {
                            Id = 6,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4321),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderHealth",
                            TargetFolderName = "Health"
                        },
                        new
                        {
                            Id = 7,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4322),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderRentAndReside",
                            TargetFolderName = "RentAndReside"
                        },
                        new
                        {
                            Id = 8,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4322),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderArchiv",
                            TargetFolderName = "Archiv"
                        },
                        new
                        {
                            Id = 9,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4323),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderSpam",
                            TargetFolderName = "Spam"
                        },
                        new
                        {
                            Id = 10,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4324),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderFamilyAndFriends",
                            TargetFolderName = "FamilyAndFriends"
                        },
                        new
                        {
                            Id = 11,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4324),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderShopping",
                            TargetFolderName = "Shopping"
                        },
                        new
                        {
                            Id = 12,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4325),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderSocialMedia",
                            TargetFolderName = "SocialMedia"
                        },
                        new
                        {
                            Id = 13,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4325),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderCar",
                            TargetFolderName = "Car"
                        },
                        new
                        {
                            Id = 14,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4326),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderTelecommunication",
                            TargetFolderName = "Telecommunication"
                        },
                        new
                        {
                            Id = 15,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4326),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderBankAndPayments",
                            TargetFolderName = "BankAndPayments"
                        },
                        new
                        {
                            Id = 16,
                            CreatedAt = new DateTime(2025, 2, 1, 14, 15, 7, 953, DateTimeKind.Utc).AddTicks(4326),
                            CreatedBy = "System",
                            ResourceKey = "labelFolderOther",
                            TargetFolderName = "Other"
                        });
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

                    b.ToTable("LogMessageTable");
                });

            modelBuilder.Entity("Data.Shared.Tools.EmailDataEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EmailDataTable");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailCleanerSettingsEntity", b =>
                {
                    b.HasOne("Data.Shared.Email.EmailAccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Data.Shared.Email.EmailCleanupConfigurationEntity", b =>
                {
                    b.HasOne("Data.Shared.Email.EmailAccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Shared.Email.EmailAddressEntity", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Shared.Email.EmailTargetFolderEntity", "PredictedTargetFolder")
                        .WithMany()
                        .HasForeignKey("PredictedTargetFolderId");

                    b.HasOne("Data.Shared.Email.EmailSubjectEntity", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Shared.Email.EmailTargetFolderEntity", "TargetFolder")
                        .WithMany()
                        .HasForeignKey("TargetFolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Address");

                    b.Navigation("PredictedTargetFolder");

                    b.Navigation("Subject");

                    b.Navigation("TargetFolder");
                });
#pragma warning restore 612, 618
        }
    }
}
