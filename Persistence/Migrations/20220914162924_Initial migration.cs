using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.Persistence.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CroppedContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CropParameters_X = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: false),
                    CropParameters_Y = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: false),
                    CropParameters_Width = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: false),
                    CropParameters_Height = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainOffer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSubscribersCount = table.Column<int>(type: "int", nullable: false),
                    Amount_Currency = table.Column<int>(type: "int", nullable: true),
                    Amount_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true, defaultValue: 0m),
                    InvoicePeriod = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxContractsPerIdentity = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainOffer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitType_Account_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainContract",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainContract_DomainOffer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "DomainOffer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainOffersPerIdentityCounter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainOffersPerIdentityCounter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainOffersPerIdentityCounter_DomainOffer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "DomainOffer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HolderTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DebtorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitTypeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPerformed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolderTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolderTransaction_Account_CreditorId",
                        column: x => x.CreditorId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransaction_Account_DebtorId",
                        column: x => x.DebtorId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransaction_UnitType_Quantity_UnitTypeId1",
                        column: x => x.Quantity_UnitTypeId1,
                        principalTable: "UnitType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CanBeExchanged = table.Column<bool>(type: "bit", nullable: false),
                    Supply = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unit_UnitType_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "UnitType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    MembersCount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domain_DomainContract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "DomainContract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountItem",
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
                    table.PrimaryKey("PK_AccountItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountItem_Account_HolderId",
                        column: x => x.HolderId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountItem_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsIssuer = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HolderTransactionItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreditAccountItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DebitAccountItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolderTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolderTransactionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolderTransactionItem_AccountItem_CreditAccountItemId",
                        column: x => x.CreditAccountItemId,
                        principalTable: "AccountItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransactionItem_AccountItem_DebitAccountItemId",
                        column: x => x.DebitAccountItemId,
                        principalTable: "AccountItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransactionItem_HolderTransaction_HolderTransactionId",
                        column: x => x.HolderTransactionId,
                        principalTable: "HolderTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransactionItem_Unit_Quantity_UnitId1",
                        column: x => x.Quantity_UnitId1,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuerTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IssuerAccountId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuerTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuerTransaction_AccountItem_IssuerAccountId1",
                        column: x => x.IssuerAccountId1,
                        principalTable: "AccountItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuerTransaction_Unit_Quantity_UnitId1",
                        column: x => x.Quantity_UnitId1,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountItem_HolderId_UnitId",
                table: "AccountItem",
                columns: new[] { "HolderId", "UnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountItem_UnitId",
                table: "AccountItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_ContractId",
                table: "Domain",
                column: "ContractId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainAccount_DomainId_IdentityId",
                table: "DomainAccount",
                columns: new[] { "DomainId", "IdentityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_DomainName",
                table: "DomainContract",
                column: "DomainName",
                unique: true,
                filter: "[DomainName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_OfferId",
                table: "DomainContract",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_PartyId",
                table: "DomainContract",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainOffersPerIdentityCounter_IdentityId",
                table: "DomainOffersPerIdentityCounter",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainOffersPerIdentityCounter_OfferId",
                table: "DomainOffersPerIdentityCounter",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainOffersPerIdentityCounter_OfferId_IdentityId",
                table: "DomainOffersPerIdentityCounter",
                columns: new[] { "OfferId", "IdentityId" });

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransaction_CreditorId",
                table: "HolderTransaction",
                column: "CreditorId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransaction_DebtorId",
                table: "HolderTransaction",
                column: "DebtorId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransaction_Quantity_UnitTypeId1",
                table: "HolderTransaction",
                column: "Quantity_UnitTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionItem_CreditAccountItemId",
                table: "HolderTransactionItem",
                column: "CreditAccountItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionItem_DebitAccountItemId",
                table: "HolderTransactionItem",
                column: "DebitAccountItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionItem_HolderTransactionId",
                table: "HolderTransactionItem",
                column: "HolderTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionItem_Quantity_UnitId1",
                table: "HolderTransactionItem",
                column: "Quantity_UnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_IssuerAccountId1",
                table: "IssuerTransaction",
                column: "IssuerAccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_Quantity_UnitId1",
                table: "IssuerTransaction",
                column: "Quantity_UnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_Login_IdentityId",
                table: "Login",
                column: "IdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_UnitTypeId_ValidFrom_ValidTo_CanBeExchanged",
                table: "Unit",
                columns: new[] { "UnitTypeId", "ValidFrom", "ValidTo", "CanBeExchanged" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitType_IssuerId",
                table: "UnitType",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValue_Ticker_DomainId",
                table: "VoucherValue",
                columns: new[] { "Ticker", "DomainId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppImage");

            migrationBuilder.DropTable(
                name: "DomainAccount");

            migrationBuilder.DropTable(
                name: "DomainOffersPerIdentityCounter");

            migrationBuilder.DropTable(
                name: "HolderTransactionItem");

            migrationBuilder.DropTable(
                name: "IssuerTransaction");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "VoucherValue");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "HolderTransaction");

            migrationBuilder.DropTable(
                name: "AccountItem");

            migrationBuilder.DropTable(
                name: "Identity");

            migrationBuilder.DropTable(
                name: "DomainContract");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "DomainOffer");

            migrationBuilder.DropTable(
                name: "UnitType");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
