using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Transactionrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolderTransactionRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DebtorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity_UnitTypeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolderTransactionRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolderTransactionRequest_Account_CreditorId",
                        column: x => x.CreditorId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransactionRequest_Account_DebtorId",
                        column: x => x.DebtorId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolderTransactionRequest_HolderTransaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "HolderTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HolderTransactionRequest_UnitType_Quantity_UnitTypeId1",
                        column: x => x.Quantity_UnitTypeId1,
                        principalTable: "UnitType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionRequest_CreditorId",
                table: "HolderTransactionRequest",
                column: "CreditorId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionRequest_DebtorId",
                table: "HolderTransactionRequest",
                column: "DebtorId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionRequest_Quantity_UnitTypeId1",
                table: "HolderTransactionRequest",
                column: "Quantity_UnitTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionRequest_TransactionId",
                table: "HolderTransactionRequest",
                column: "TransactionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolderTransactionRequest");
        }
    }
}
