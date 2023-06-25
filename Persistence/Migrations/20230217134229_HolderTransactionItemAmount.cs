using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vouchers.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HolderTransactionItemAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_Unit_Quantity_UnitId",
                table: "HolderTransactionItem");

            migrationBuilder.DropIndex(
                name: "IX_HolderTransactionItem_Quantity_UnitId",
                table: "HolderTransactionItem");

            migrationBuilder.DropColumn(
                name: "Quantity_UnitId",
                table: "HolderTransactionItem");

            migrationBuilder.RenameColumn(
                name: "Quantity_Amount",
                table: "HolderTransactionItem",
                newName: "Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "HolderTransactionItem",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "HolderTransactionItem",
                newName: "Quantity_Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity_Amount",
                table: "HolderTransactionItem",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<Guid>(
                name: "Quantity_UnitId",
                table: "HolderTransactionItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolderTransactionItem_Quantity_UnitId",
                table: "HolderTransactionItem",
                column: "Quantity_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_Unit_Quantity_UnitId",
                table: "HolderTransactionItem",
                column: "Quantity_UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
