using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class HolderTransactionUnitTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_UnitType_Quantity_UnitTypeId1",
                table: "HolderTransaction");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitTypeId1",
                table: "HolderTransaction",
                newName: "Quantity_UnitTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_Quantity_UnitTypeId1",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_Quantity_UnitTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_UnitType_Quantity_UnitTypeId",
                table: "HolderTransaction",
                column: "Quantity_UnitTypeId",
                principalTable: "UnitType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_UnitType_Quantity_UnitTypeId",
                table: "HolderTransaction");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitTypeId",
                table: "HolderTransaction",
                newName: "Quantity_UnitTypeId1");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_Quantity_UnitTypeId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_Quantity_UnitTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_UnitType_Quantity_UnitTypeId1",
                table: "HolderTransaction",
                column: "Quantity_UnitTypeId1",
                principalTable: "UnitType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
