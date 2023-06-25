using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vouchers.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IssuerTransactionAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_Unit_Quantity_UnitId",
                table: "IssuerTransaction");

            migrationBuilder.DropIndex(
                name: "IX_IssuerTransaction_Quantity_UnitId",
                table: "IssuerTransaction");

            migrationBuilder.DropColumn(
                name: "Quantity_UnitId",
                table: "IssuerTransaction");

            migrationBuilder.RenameColumn(
                name: "Quantity_Amount",
                table: "IssuerTransaction",
                newName: "Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "IssuerTransaction",
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
                table: "IssuerTransaction",
                newName: "Quantity_Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity_Amount",
                table: "IssuerTransaction",
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
                table: "IssuerTransaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_Quantity_UnitId",
                table: "IssuerTransaction",
                column: "Quantity_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_Unit_Quantity_UnitId",
                table: "IssuerTransaction",
                column: "Quantity_UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
