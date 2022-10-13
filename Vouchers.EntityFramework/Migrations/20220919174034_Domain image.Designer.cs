﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vouchers.EntityFramework;

namespace Vouchers.EntityFramework.Migrations
{
    [DbContext(typeof(VouchersDbContext))]
    [Migration("20220919174034_Domain image")]
    partial class Domainimage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Vouchers.Core.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<decimal>("Supply")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0m);

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Vouchers.Core.AccountItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("HolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("UnitId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UnitId");

                    b.HasIndex("HolderId", "UnitId");

                    b.ToTable("AccountItem");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreditorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DebtorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPerformed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreditorId");

                    b.HasIndex("DebtorId");

                    b.ToTable("HolderTransaction");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransactionItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreditAccountItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DebitAccountItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("HolderTransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreditAccountItemId");

                    b.HasIndex("DebitAccountItemId");

                    b.HasIndex("HolderTransactionId");

                    b.ToTable("HolderTransactionItem");
                });

            modelBuilder.Entity("Vouchers.Core.IssuerTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IssuerAccountId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IssuerAccountId1");

                    b.ToTable("IssuerTransaction");
                });

            modelBuilder.Entity("Vouchers.Core.Unit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanBeExchanged")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<decimal>("Supply")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("UnitTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UnitTypeId", "ValidFrom", "ValidTo", "CanBeExchanged")
                        .IsUnique();

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("Vouchers.Core.UnitType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IssuerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<decimal>("Supply")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IssuerId");

                    b.ToTable("UnitType");
                });

            modelBuilder.Entity("Vouchers.Domains.Domain", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Credit")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0m);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<int>("MembersCount")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ContractId")
                        .IsUnique();

                    b.ToTable("Domain");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIssuer")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("DomainId", "IdentityId")
                        .IsUnique();

                    b.ToTable("DomainAccount");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainContract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DomainName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("OfferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OffersPerIdentityCounterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PartyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("DomainName")
                        .IsUnique()
                        .HasFilter("[DomainName] IS NOT NULL");

                    b.HasIndex("OfferId");

                    b.HasIndex("OffersPerIdentityCounterId")
                        .IsUnique();

                    b.HasIndex("PartyId");

                    b.ToTable("DomainContract");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvoicePeriod")
                        .HasColumnType("int");

                    b.Property<int?>("MaxContractsPerIdentity")
                        .HasColumnType("int");

                    b.Property<int>("MaxSubscribersCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RecipientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DomainOffer");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainOffersPerIdentityCounter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OfferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.HasIndex("OfferId");

                    b.HasIndex("OfferId", "IdentityId");

                    b.ToTable("DomainOffersPerIdentityCounter");
                });

            modelBuilder.Entity("Vouchers.Files.AppImage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("CroppedContent")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("AppImage");
                });

            modelBuilder.Entity("Vouchers.Identities.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Identity");
                });

            modelBuilder.Entity("Vouchers.Identities.Login", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId")
                        .IsUnique();

                    b.ToTable("Login");
                });

            modelBuilder.Entity("Vouchers.Values.VoucherValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IssuerIdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Ticker", "DomainId")
                        .IsUnique();

                    b.ToTable("VoucherValue");
                });

            modelBuilder.Entity("Vouchers.Core.AccountItem", b =>
                {
                    b.HasOne("Vouchers.Core.Account", "Holder")
                        .WithMany()
                        .HasForeignKey("HolderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Holder");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransaction", b =>
                {
                    b.HasOne("Vouchers.Core.Account", "Creditor")
                        .WithMany()
                        .HasForeignKey("CreditorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Account", "Debtor")
                        .WithMany()
                        .HasForeignKey("DebtorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Vouchers.Core.UnitTypeQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid?>("UnitTypeId1")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionId");

                            b1.HasIndex("UnitTypeId1");

                            b1.ToTable("HolderTransaction");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionId");

                            b1.HasOne("Vouchers.Core.UnitType", "UnitType")
                                .WithMany()
                                .HasForeignKey("UnitTypeId1")
                                .OnDelete(DeleteBehavior.Restrict);

                            b1.Navigation("UnitType");
                        });

                    b.Navigation("Creditor");

                    b.Navigation("Debtor");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransactionItem", b =>
                {
                    b.HasOne("Vouchers.Core.AccountItem", "CreditAccountItem")
                        .WithMany()
                        .HasForeignKey("CreditAccountItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.AccountItem", "DebitAccountItem")
                        .WithMany()
                        .HasForeignKey("DebitAccountItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.HolderTransaction", null)
                        .WithMany("TransactionItems")
                        .HasForeignKey("HolderTransactionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Vouchers.Core.UnitQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid?>("UnitId1")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionItemId");

                            b1.HasIndex("UnitId1");

                            b1.ToTable("HolderTransactionItem");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionItemId");

                            b1.HasOne("Vouchers.Core.Unit", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId1")
                                .OnDelete(DeleteBehavior.Restrict);

                            b1.Navigation("Unit");
                        });

                    b.Navigation("CreditAccountItem");

                    b.Navigation("DebitAccountItem");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.IssuerTransaction", b =>
                {
                    b.HasOne("Vouchers.Core.AccountItem", "IssuerAccount")
                        .WithMany()
                        .HasForeignKey("IssuerAccountId1")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Vouchers.Core.UnitQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("IssuerTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid?>("UnitId1")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("IssuerTransactionId");

                            b1.HasIndex("UnitId1");

                            b1.ToTable("IssuerTransaction");

                            b1.WithOwner()
                                .HasForeignKey("IssuerTransactionId");

                            b1.HasOne("Vouchers.Core.Unit", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId1")
                                .OnDelete(DeleteBehavior.Restrict);

                            b1.Navigation("Unit");
                        });

                    b.Navigation("IssuerAccount");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.Unit", b =>
                {
                    b.HasOne("Vouchers.Core.UnitType", "UnitType")
                        .WithMany()
                        .HasForeignKey("UnitTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UnitType");
                });

            modelBuilder.Entity("Vouchers.Core.UnitType", b =>
                {
                    b.HasOne("Vouchers.Core.Account", "Issuer")
                        .WithMany()
                        .HasForeignKey("IssuerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Issuer");
                });

            modelBuilder.Entity("Vouchers.Domains.Domain", b =>
                {
                    b.HasOne("Vouchers.Domains.DomainContract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainAccount", b =>
                {
                    b.HasOne("Vouchers.Domains.Domain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainContract", b =>
                {
                    b.HasOne("Vouchers.Domains.DomainOffer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Domains.DomainOffersPerIdentityCounter", "OffersPerIdentityCounter")
                        .WithMany()
                        .HasForeignKey("OffersPerIdentityCounterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Offer");

                    b.Navigation("OffersPerIdentityCounter");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainOffer", b =>
                {
                    b.OwnsOne("Vouchers.Domains.CurrencyAmount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("DomainOfferId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .ValueGeneratedOnAdd()
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)")
                                .HasDefaultValue(0m);

                            b1.Property<int>("Currency")
                                .HasColumnType("int");

                            b1.HasKey("DomainOfferId");

                            b1.ToTable("DomainOffer");

                            b1.WithOwner()
                                .HasForeignKey("DomainOfferId");
                        });

                    b.Navigation("Amount");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainOffersPerIdentityCounter", b =>
                {
                    b.HasOne("Vouchers.Domains.DomainOffer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("Vouchers.Files.AppImage", b =>
                {
                    b.OwnsOne("Vouchers.Files.CropParameters", "CropParameters", b1 =>
                        {
                            b1.Property<Guid>("AppImageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Height")
                                .HasPrecision(18, 15)
                                .HasColumnType("decimal(18,15)");

                            b1.Property<decimal>("Width")
                                .HasPrecision(18, 15)
                                .HasColumnType("decimal(18,15)");

                            b1.Property<decimal>("X")
                                .HasPrecision(18, 15)
                                .HasColumnType("decimal(18,15)");

                            b1.Property<decimal>("Y")
                                .HasPrecision(18, 15)
                                .HasColumnType("decimal(18,15)");

                            b1.HasKey("AppImageId");

                            b1.ToTable("AppImage");

                            b1.WithOwner()
                                .HasForeignKey("AppImageId");
                        });

                    b.Navigation("CropParameters")
                        .IsRequired();
                });

            modelBuilder.Entity("Vouchers.Identities.Login", b =>
                {
                    b.HasOne("Vouchers.Identities.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransaction", b =>
                {
                    b.Navigation("TransactionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
