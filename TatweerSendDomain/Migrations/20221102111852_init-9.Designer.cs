﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TatweerSendDomain;

#nullable disable

namespace TatweerSendDomain.Migrations
{
    [DbContext(typeof(TatweerSendDbContext))]
    [Migration("20221102111852_init-9")]
    partial class init9
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TatweerSendDomain.Domain.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccountState")
                        .HasColumnType("int");

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<string>("BranchId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Bank", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("BankNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BankRegion", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("BankId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("RegionId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("RegionId");

                    b.ToTable("BankRegions");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Branch", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("BranchNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchRegionId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<long>("LastSerial")
                        .HasColumnType("bigint");

                    b.Property<long>("LastSerialCertified")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("BranchRegionId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BranchSetting", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("AccountChekBook")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("000000000000000");

                    b.Property<string>("BranchId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("CertifiedFrom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("CertifiedTo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(100);

                    b.Property<int>("CompanyFrom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("CompanyTo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(100);

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<int>("IndividualFrom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("IndividualQuentityOfDay")
                        .HasColumnType("int");

                    b.Property<bool>("IndividualRequestAccountDay")
                        .HasColumnType("bit");

                    b.Property<int>("IndividualTo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(100);

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("BranchId")
                        .IsUnique()
                        .HasFilter("[BranchId] IS NOT NULL");

                    b.ToTable("BranchSettings");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BranchWorkTime", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("BranchId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<int>("DayName")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("TimeEnd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeStart")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("BranchWorkTimes");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountChekBook")
                        .HasColumnType("int");

                    b.Property<string>("OrderRequestId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("SerialFrom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrderRequestId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderRequest", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("BranchId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("EmployeeNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityNumberBank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifyDate")
                        .HasColumnType("datetime");

                    b.Property<int>("OrderRequestState")
                        .HasColumnType("int");

                    b.Property<int>("OrderRequestType")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("OrderRequests");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderSend", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("EmployeeNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderRequestId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("OrderSendDate")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderRequestId")
                        .IsUnique();

                    b.ToTable("OrderSends");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Region", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Account", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.Branch", "Branch")
                        .WithMany("Accounts")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_Accounts");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BankRegion", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.Bank", "Bank")
                        .WithMany("BankRegions")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Region_Bank");

                    b.HasOne("TatweerSendDomain.Domain.Region", "Region")
                        .WithMany("BankRegions")
                        .HasForeignKey("RegionId")
                        .HasConstraintName("FK_Region_Bank_Region");

                    b.Navigation("Bank");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Branch", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.BankRegion", "BranchRegion")
                        .WithMany("Branchs")
                        .HasForeignKey("BranchRegionId")
                        .HasConstraintName("FK_BRANCH_REGION");

                    b.Navigation("BranchRegion");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BranchSetting", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.Branch", "Branch")
                        .WithOne("BranchSetting")
                        .HasForeignKey("TatweerSendDomain.Domain.BranchSetting", "BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BRANCH_Branch_Setting");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BranchWorkTime", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.Branch", "Branch")
                        .WithMany("BranchWorkTimes")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_Branch_Work_Times");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderItem", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.Account", "Account")
                        .WithMany("OrderItems")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TatweerSendDomain.Domain.OrderRequest", "OrderRequest")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderItemt_Branchs");

                    b.Navigation("Account");

                    b.Navigation("OrderRequest");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderRequest", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.Branch", "Branch")
                        .WithMany("OrderRequests")
                        .HasForeignKey("BranchId")
                        .HasConstraintName("FK_OrderRequest_Branchs");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderSend", b =>
                {
                    b.HasOne("TatweerSendDomain.Domain.OrderRequest", "OrderRequest")
                        .WithOne("OrderSend")
                        .HasForeignKey("TatweerSendDomain.Domain.OrderSend", "OrderRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderSend_Branchs");

                    b.Navigation("OrderRequest");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Account", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Bank", b =>
                {
                    b.Navigation("BankRegions");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.BankRegion", b =>
                {
                    b.Navigation("Branchs");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Branch", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("BranchSetting");

                    b.Navigation("BranchWorkTimes");

                    b.Navigation("OrderRequests");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.OrderRequest", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("OrderSend");
                });

            modelBuilder.Entity("TatweerSendDomain.Domain.Region", b =>
                {
                    b.Navigation("BankRegions");
                });
#pragma warning restore 612, 618
        }
    }
}
