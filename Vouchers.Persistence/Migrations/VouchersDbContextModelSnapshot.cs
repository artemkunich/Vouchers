﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vouchers.Persistence;

#nullable disable

namespace Vouchers.Persistence.Migrations
{
    [DbContext(typeof(VouchersDbContext))]
    partial class VouchersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Vouchers.Core.Domain.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

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

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.AccountItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("HolderAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("UnitId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UnitId");

                    b.HasIndex("HolderAccountId", "UnitId");

                    b.ToTable("AccountItem", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreditorAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DebtorAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPerformed")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreditorAccountId");

                    b.HasIndex("DebtorAccountId");

                    b.ToTable("HolderTransaction", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransactionItem", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.ToTable("HolderTransactionItem", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransactionRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreditorAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DebtorAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("MaxDurationBeforeValidityStart")
                        .HasColumnType("time");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("MinDurationBeforeValidityEnd")
                        .HasColumnType("time");

                    b.Property<bool>("MustBeExchangeable")
                        .HasColumnType("bit");

                    b.Property<Guid?>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreditorAccountId");

                    b.HasIndex("DebtorAccountId");

                    b.HasIndex("TransactionId")
                        .IsUnique()
                        .HasFilter("[TransactionId] IS NOT NULL");

                    b.ToTable("HolderTransactionRequest", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.IssuerTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IssuerAccountItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IssuerAccountItemId");

                    b.ToTable("IssuerTransaction", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.Unit", b =>
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

                    b.ToTable("Unit", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.UnitType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IssuerAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<decimal>("Supply")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IssuerAccountId");

                    b.ToTable("UnitType", (string)null);
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.Domain", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

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

                    b.ToTable("Domain", (string)null);
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

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

                    b.ToTable("DomainAccount", (string)null);
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainContract", b =>
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

                    b.ToTable("DomainContract", (string)null);
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvoicePeriod")
                        .HasColumnType("int");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxContractsPerIdentity")
                        .HasColumnType("int");

                    b.Property<int>("MaxMembersCount")
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

                    b.ToTable("DomainOffer", (string)null);
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainOffersPerIdentityCounter", b =>
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

                    b.ToTable("DomainOffersPerIdentityCounter", (string)null);
                });

            modelBuilder.Entity("Vouchers.Files.Domain.CroppedImage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("CroppedImage", (string)null);
                });

            modelBuilder.Entity("Vouchers.Identities.Domain.Identity", b =>
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

                    b.ToTable("Identity", (string)null);
                });

            modelBuilder.Entity("Vouchers.Identities.Domain.Login", b =>
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

                    b.ToTable("Login", (string)null);
                });

            modelBuilder.Entity("Vouchers.Persistence.InterCommunication.ConsumedMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ConsumedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Consumer")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("MessageId", "Consumer")
                        .IsUnique();

                    b.ToTable("ConsumedMessage", (string)null);
                });

            modelBuilder.Entity("Vouchers.Persistence.InterCommunication.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessage", (string)null);
                });

            modelBuilder.Entity("Vouchers.Values.Domain.VoucherValue", b =>
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

                    b.ToTable("VoucherValue", (string)null);
                });

            modelBuilder.Entity("Vouchers.Core.Domain.AccountItem", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.Account", "HolderAccount")
                        .WithMany()
                        .HasForeignKey("HolderAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Domain.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("HolderAccount");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransaction", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.Account", "CreditorAccount")
                        .WithMany()
                        .HasForeignKey("CreditorAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Domain.Account", "DebtorAccount")
                        .WithMany()
                        .HasForeignKey("DebtorAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Vouchers.Core.Domain.UnitTypeQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid>("UnitTypeId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionId");

                            b1.HasIndex("UnitTypeId");

                            b1.ToTable("HolderTransaction");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionId");

                            b1.HasOne("Vouchers.Core.Domain.UnitType", "UnitType")
                                .WithMany()
                                .HasForeignKey("UnitTypeId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired();

                            b1.Navigation("UnitType");
                        });

                    b.Navigation("CreditorAccount");

                    b.Navigation("DebtorAccount");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransactionItem", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.AccountItem", "CreditAccountItem")
                        .WithMany()
                        .HasForeignKey("CreditAccountItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Domain.AccountItem", "DebitAccountItem")
                        .WithMany()
                        .HasForeignKey("DebitAccountItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Domain.HolderTransaction", null)
                        .WithMany("TransactionItems")
                        .HasForeignKey("HolderTransactionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Vouchers.Core.Domain.UnitQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid>("UnitId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionItemId");

                            b1.HasIndex("UnitId");

                            b1.ToTable("HolderTransactionItem");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionItemId");

                            b1.HasOne("Vouchers.Core.Domain.Unit", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired();

                            b1.Navigation("Unit");
                        });

                    b.Navigation("CreditAccountItem");

                    b.Navigation("DebitAccountItem");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransactionRequest", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.Account", "CreditorAccount")
                        .WithMany()
                        .HasForeignKey("CreditorAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Vouchers.Core.Domain.Account", "DebtorAccount")
                        .WithMany()
                        .HasForeignKey("DebtorAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Domain.HolderTransaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Vouchers.Core.Domain.UnitTypeQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionRequestId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid>("UnitTypeId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionRequestId");

                            b1.HasIndex("UnitTypeId");

                            b1.ToTable("HolderTransactionRequest");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionRequestId");

                            b1.HasOne("Vouchers.Core.Domain.UnitType", "UnitType")
                                .WithMany()
                                .HasForeignKey("UnitTypeId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired();

                            b1.Navigation("UnitType");
                        });

                    b.Navigation("CreditorAccount");

                    b.Navigation("DebtorAccount");

                    b.Navigation("Quantity");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.IssuerTransaction", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.AccountItem", "IssuerAccountItem")
                        .WithMany()
                        .HasForeignKey("IssuerAccountItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Vouchers.Core.Domain.UnitQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("IssuerTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid>("UnitId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("IssuerTransactionId");

                            b1.HasIndex("UnitId");

                            b1.ToTable("IssuerTransaction");

                            b1.WithOwner()
                                .HasForeignKey("IssuerTransactionId");

                            b1.HasOne("Vouchers.Core.Domain.Unit", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired();

                            b1.Navigation("Unit");
                        });

                    b.Navigation("IssuerAccountItem");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.Unit", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.UnitType", "UnitType")
                        .WithMany()
                        .HasForeignKey("UnitTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UnitType");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.UnitType", b =>
                {
                    b.HasOne("Vouchers.Core.Domain.Account", "IssuerAccount")
                        .WithMany()
                        .HasForeignKey("IssuerAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IssuerAccount");
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.Domain", b =>
                {
                    b.HasOne("Vouchers.Domains.Domain.DomainContract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainAccount", b =>
                {
                    b.HasOne("Vouchers.Domains.Domain.Domain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainContract", b =>
                {
                    b.HasOne("Vouchers.Domains.Domain.DomainOffer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vouchers.Domains.Domain.DomainOffersPerIdentityCounter", "OffersPerIdentityCounter")
                        .WithMany()
                        .HasForeignKey("OffersPerIdentityCounterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Offer");

                    b.Navigation("OffersPerIdentityCounter");
                });

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainOffer", b =>
                {
                    b.OwnsOne("Vouchers.Domains.Domain.CurrencyAmount", "Amount", b1 =>
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

            modelBuilder.Entity("Vouchers.Domains.Domain.DomainOffersPerIdentityCounter", b =>
                {
                    b.HasOne("Vouchers.Domains.Domain.DomainOffer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("Vouchers.Files.Domain.CroppedImage", b =>
                {
                    b.OwnsOne("Vouchers.Files.Domain.CropParameters", "CropParameters", b1 =>
                        {
                            b1.Property<Guid>("CroppedImageId")
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

                            b1.HasKey("CroppedImageId");

                            b1.ToTable("CroppedImage");

                            b1.WithOwner()
                                .HasForeignKey("CroppedImageId");
                        });

                    b.Navigation("CropParameters")
                        .IsRequired();
                });

            modelBuilder.Entity("Vouchers.Identities.Domain.Login", b =>
                {
                    b.HasOne("Vouchers.Identities.Domain.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("Vouchers.Core.Domain.HolderTransaction", b =>
                {
                    b.Navigation("TransactionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
