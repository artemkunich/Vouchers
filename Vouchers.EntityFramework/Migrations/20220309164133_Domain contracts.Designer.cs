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
    [Migration("20220309164133_Domain contracts")]
    partial class Domaincontracts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Vouchers.Core.Domain", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MembersCount")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Domain");
                });

            modelBuilder.Entity("Vouchers.Core.DomainAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DomainId");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdentityId");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIssuer")
                        .HasColumnType("bit");

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

                    b.HasIndex("IdentityId");

                    b.HasIndex("DomainId", "IdentityId")
                        .IsUnique();

                    b.ToTable("DomainAccount");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreditorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DebtorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreditorId");

                    b.HasIndex("DebtorId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransactionItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreditAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DebitAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("HolderTransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreditAccountId");

                    b.HasIndex("DebitAccountId");

                    b.HasIndex("HolderTransactionId");

                    b.ToTable("TransactionItem");
                });

            modelBuilder.Entity("Vouchers.Core.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Identity");
                });

            modelBuilder.Entity("Vouchers.Core.IssuerTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IssuerAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IssuerAccountId");

                    b.ToTable("IssuerTransaction");
                });

            modelBuilder.Entity("Vouchers.Core.Voucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanBeExchanged")
                        .IsConcurrencyToken()
                        .HasColumnType("bit")
                        .HasColumnName("CanBeExchanged");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<decimal>("Supply")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Supply");

                    b.Property<DateTime>("ValidFrom")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidFrom");

                    b.Property<DateTime>("ValidTo")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2")
                        .HasColumnName("ValidTo");

                    b.Property<Guid>("ValueId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ValueId");

                    b.HasKey("Id");

                    b.HasIndex("ValueId", "ValidFrom", "ValidTo", "CanBeExchanged")
                        .IsUnique();

                    b.ToTable("Voucher");
                });

            modelBuilder.Entity("Vouchers.Core.VoucherAccount", b =>
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

                    b.ToTable("VoucherAccount");
                });

            modelBuilder.Entity("Vouchers.Core.VoucherValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IssuerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IssuerId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<decimal>("Supply")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Supply");

                    b.HasKey("Id");

                    b.HasIndex("IssuerId");

                    b.ToTable("VoucherValue");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainContract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContractNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DomainId");

                    b.Property<string>("DomainName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OfferId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("OfferId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("DomainId")
                        .IsUnique();

                    b.HasIndex("OfferId")
                        .IsUnique();

                    b.ToTable("DomainContract");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ContractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DomainId");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("DomainId")
                        .IsUnique();

                    b.ToTable("DomainDetail");
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

                    b.Property<int>("MaxSubscribersCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("Vouchers.Identities.IdentityDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdentityId");

                    b.Property<string>("IdentityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId")
                        .IsUnique();

                    b.ToTable("IdentityDetail");
                });

            modelBuilder.Entity("Vouchers.Identities.Login", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdentityId");

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

            modelBuilder.Entity("Vouchers.Values.VoucherValueDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DomainId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ValueId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ValueId");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.HasIndex("ValueId")
                        .IsUnique();

                    b.HasIndex("Ticker", "DomainId")
                        .IsUnique();

                    b.ToTable("VoucherValueDetail");
                });

            modelBuilder.Entity("Vouchers.Core.DomainAccount", b =>
                {
                    b.HasOne("Vouchers.Core.Domain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Domain");

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransaction", b =>
                {
                    b.HasOne("Vouchers.Core.DomainAccount", "Creditor")
                        .WithMany()
                        .HasForeignKey("CreditorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Vouchers.Core.DomainAccount", "Debtor")
                        .WithMany()
                        .HasForeignKey("DebtorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.OwnsOne("Vouchers.Core.VoucherValueQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid?>("UnitId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionId");

                            b1.HasIndex("UnitId");

                            b1.ToTable("Transaction");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionId");

                            b1.HasOne("Vouchers.Core.VoucherValue", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.Navigation("Unit");
                        });

                    b.Navigation("Creditor");

                    b.Navigation("Debtor");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransactionItem", b =>
                {
                    b.HasOne("Vouchers.Core.VoucherAccount", "CreditAccount")
                        .WithMany()
                        .HasForeignKey("CreditAccountId");

                    b.HasOne("Vouchers.Core.VoucherAccount", "DebitAccount")
                        .WithMany()
                        .HasForeignKey("DebitAccountId");

                    b.HasOne("Vouchers.Core.HolderTransaction", null)
                        .WithMany("TransactionItems")
                        .HasForeignKey("HolderTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Vouchers.Core.VoucherQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("HolderTransactionItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid?>("UnitId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("HolderTransactionItemId");

                            b1.HasIndex("UnitId");

                            b1.ToTable("TransactionItem");

                            b1.WithOwner()
                                .HasForeignKey("HolderTransactionItemId");

                            b1.HasOne("Vouchers.Core.Voucher", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId");

                            b1.Navigation("Unit");
                        });

                    b.Navigation("CreditAccount");

                    b.Navigation("DebitAccount");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.IssuerTransaction", b =>
                {
                    b.HasOne("Vouchers.Core.VoucherAccount", "IssuerAccount")
                        .WithMany()
                        .HasForeignKey("IssuerAccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.OwnsOne("Vouchers.Core.VoucherQuantity", "Quantity", b1 =>
                        {
                            b1.Property<Guid>("IssuerTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 2)
                                .HasColumnType("decimal(18,2)");

                            b1.Property<Guid?>("UnitId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("IssuerTransactionId");

                            b1.HasIndex("UnitId");

                            b1.ToTable("IssuerTransaction");

                            b1.WithOwner()
                                .HasForeignKey("IssuerTransactionId");

                            b1.HasOne("Vouchers.Core.Voucher", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.Navigation("Unit");
                        });

                    b.Navigation("IssuerAccount");

                    b.Navigation("Quantity");
                });

            modelBuilder.Entity("Vouchers.Core.Voucher", b =>
                {
                    b.HasOne("Vouchers.Core.VoucherValue", "Value")
                        .WithMany()
                        .HasForeignKey("ValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Value");
                });

            modelBuilder.Entity("Vouchers.Core.VoucherAccount", b =>
                {
                    b.HasOne("Vouchers.Core.DomainAccount", "Holder")
                        .WithMany()
                        .HasForeignKey("HolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Voucher", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Holder");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Vouchers.Core.VoucherValue", b =>
                {
                    b.HasOne("Vouchers.Core.DomainAccount", "Issuer")
                        .WithMany()
                        .HasForeignKey("IssuerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issuer");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainContract", b =>
                {
                    b.HasOne("Vouchers.Core.Domain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vouchers.Domains.DomainOffer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Domain");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainDetail", b =>
                {
                    b.HasOne("Vouchers.Domains.DomainContract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId");

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Vouchers.Domains.DomainOffer", b =>
                {
                    b.OwnsOne("Vouchers.Domains.CurrencyAmount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("DomainOfferId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int>("Currency")
                                .HasColumnType("int");

                            b1.HasKey("DomainOfferId");

                            b1.ToTable("DomainOffer");

                            b1.WithOwner()
                                .HasForeignKey("DomainOfferId");
                        });

                    b.Navigation("Amount");
                });

            modelBuilder.Entity("Vouchers.Identities.IdentityDetail", b =>
                {
                    b.HasOne("Vouchers.Core.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("Vouchers.Identities.Login", b =>
                {
                    b.HasOne("Vouchers.Core.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("Vouchers.Values.VoucherValueDetail", b =>
                {
                    b.HasOne("Vouchers.Core.Domain", null)
                        .WithMany()
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vouchers.Core.Identity", null)
                        .WithMany()
                        .HasForeignKey("ValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vouchers.Core.HolderTransaction", b =>
                {
                    b.Navigation("TransactionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
