using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MembersCount = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsIssuer = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainAccount_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainAccount_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityDetail_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Login_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherValueDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    VoucherValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherValueDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherValueDetail_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoucherValueDetail_Identity_VoucherValueId",
                        column: x => x.VoucherValueId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherValue_DomainAccount_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "DomainAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DebtorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_DomainAccount_CreditorId",
                        column: x => x.CreditorId,
                        principalTable: "DomainAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_DomainAccount_DebtorId",
                        column: x => x.DebtorId,
                        principalTable: "DomainAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_VoucherValue_Quantity_UnitId",
                        column: x => x.Quantity_UnitId,
                        principalTable: "VoucherValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CanBeExchanged = table.Column<bool>(type: "bit", nullable: false),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voucher_VoucherValue_ValueId",
                        column: x => x.ValueId,
                        principalTable: "VoucherValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherAccount_DomainAccount_HolderId",
                        column: x => x.HolderId,
                        principalTable: "DomainAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoucherAccount_Voucher_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Voucher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IssuerTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IssuerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuerTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuerTransaction_Voucher_Quantity_UnitId",
                        column: x => x.Quantity_UnitId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuerTransaction_VoucherAccount_IssuerAccountId",
                        column: x => x.IssuerAccountId,
                        principalTable: "VoucherAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreditAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DebitAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HolderTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItem_Transaction_HolderTransactionId",
                        column: x => x.HolderTransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionItem_Voucher_Quantity_UnitId",
                        column: x => x.Quantity_UnitId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItem_VoucherAccount_CreditAccountId",
                        column: x => x.CreditAccountId,
                        principalTable: "VoucherAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItem_VoucherAccount_DebitAccountId",
                        column: x => x.DebitAccountId,
                        principalTable: "VoucherAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainAccount_DomainId_IdentityId",
                table: "DomainAccount",
                columns: new[] { "DomainId", "IdentityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainAccount_IdentityId",
                table: "DomainAccount",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityDetail_IdentityId",
                table: "IdentityDetail",
                column: "IdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_IssuerAccountId",
                table: "IssuerTransaction",
                column: "IssuerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_Quantity_UnitId",
                table: "IssuerTransaction",
                column: "Quantity_UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Login_IdentityId",
                table: "Login",
                column: "IdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreditorId",
                table: "Transaction",
                column: "CreditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DebtorId",
                table: "Transaction",
                column: "DebtorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Quantity_UnitId",
                table: "Transaction",
                column: "Quantity_UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_CreditAccountId",
                table: "TransactionItem",
                column: "CreditAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_DebitAccountId",
                table: "TransactionItem",
                column: "DebitAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_HolderTransactionId",
                table: "TransactionItem",
                column: "HolderTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_Quantity_UnitId",
                table: "TransactionItem",
                column: "Quantity_UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_ValueId_ValidFrom_ValidTo_CanBeExchanged",
                table: "Voucher",
                columns: new[] { "ValueId", "ValidFrom", "ValidTo", "CanBeExchanged" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherAccount_HolderId_UnitId",
                table: "VoucherAccount",
                columns: new[] { "HolderId", "UnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_VoucherAccount_UnitId",
                table: "VoucherAccount",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValue_IssuerId",
                table: "VoucherValue",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValueDetail_DomainId",
                table: "VoucherValueDetail",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValueDetail_Ticker_DomainId",
                table: "VoucherValueDetail",
                columns: new[] { "Ticker", "DomainId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValueDetail_VoucherValueId",
                table: "VoucherValueDetail",
                column: "VoucherValueId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityDetail");

            migrationBuilder.DropTable(
                name: "IssuerTransaction");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "TransactionItem");

            migrationBuilder.DropTable(
                name: "VoucherValueDetail");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "VoucherAccount");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "VoucherValue");

            migrationBuilder.DropTable(
                name: "DomainAccount");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "Identity");
        }
    }
}
