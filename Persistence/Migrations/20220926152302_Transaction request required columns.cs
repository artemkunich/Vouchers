using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.Persistence.Migrations
{
    public partial class Transactionrequestrequiredcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_HolderTransaction_TransactionId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_UnitType_Quantity_UnitTypeId1",
                table: "HolderTransactionRequest");

            migrationBuilder.DropIndex(
                name: "IX_HolderTransactionRequest_TransactionId",
                table: "HolderTransactionRequest");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitTypeId1",
                table: "HolderTransactionRequest",
                newName: "Quantity_UnitTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionRequest_Quantity_UnitTypeId1",
                table: "HolderTransactionRequest",
                newName: "IX_HolderTransactionRequest_Quantity_UnitTypeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionId",
                table: "HolderTransactionRequest",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreditorId",
                table: "HolderTransactionRequest",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionRequest_TransactionId",
                table: "HolderTransactionRequest",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_HolderTransaction_TransactionId",
                table: "HolderTransactionRequest",
                column: "TransactionId",
                principalTable: "HolderTransaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_UnitType_Quantity_UnitTypeId",
                table: "HolderTransactionRequest",
                column: "Quantity_UnitTypeId",
                principalTable: "UnitType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_HolderTransaction_TransactionId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_UnitType_Quantity_UnitTypeId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropIndex(
                name: "IX_HolderTransactionRequest_TransactionId",
                table: "HolderTransactionRequest");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitTypeId",
                table: "HolderTransactionRequest",
                newName: "Quantity_UnitTypeId1");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionRequest_Quantity_UnitTypeId",
                table: "HolderTransactionRequest",
                newName: "IX_HolderTransactionRequest_Quantity_UnitTypeId1");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionId",
                table: "HolderTransactionRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreditorId",
                table: "HolderTransactionRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionRequest_TransactionId",
                table: "HolderTransactionRequest",
                column: "TransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_HolderTransaction_TransactionId",
                table: "HolderTransactionRequest",
                column: "TransactionId",
                principalTable: "HolderTransaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_UnitType_Quantity_UnitTypeId1",
                table: "HolderTransactionRequest",
                column: "Quantity_UnitTypeId1",
                principalTable: "UnitType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
