using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vouchers.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HiddenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_Unit_Quantity_UnitId1",
                table: "HolderTransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountItemId1",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_Unit_Quantity_UnitId1",
                table: "IssuerTransaction");

            migrationBuilder.DropIndex(
                name: "IX_IssuerTransaction_IssuerAccountItemId1",
                table: "IssuerTransaction");
            
            migrationBuilder.RenameColumn(
                name: "IssuerAccountItemId1",
                table: "IssuerTransaction",
                newName: "IssuerAccountItemId");
            
            migrationBuilder.RenameColumn(
                name: "Quantity_UnitId1",
                table: "IssuerTransaction",
                newName: "Quantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_IssuerTransaction_Quantity_UnitId1",
                table: "IssuerTransaction",
                newName: "IX_IssuerTransaction_Quantity_UnitId");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitId1",
                table: "HolderTransactionItem",
                newName: "Quantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionItem_Quantity_UnitId1",
                table: "HolderTransactionItem",
                newName: "IX_HolderTransactionItem_Quantity_UnitId");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxDurationBeforeValidityStart",
                table: "HolderTransactionRequest",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MinDurationBeforeValidityEnd",
                table: "HolderTransactionRequest",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MustBeExchangeable",
                table: "HolderTransactionRequest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "DomainOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DomainAccount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Domain",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Account",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_IssuerAccountItemId",
                table: "IssuerTransaction",
                column: "IssuerAccountItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_Unit_Quantity_UnitId",
                table: "HolderTransactionItem",
                column: "Quantity_UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountItemId",
                table: "IssuerTransaction",
                column: "IssuerAccountItemId",
                principalTable: "AccountItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_Unit_Quantity_UnitId",
                table: "IssuerTransaction",
                column: "Quantity_UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_Unit_Quantity_UnitId",
                table: "HolderTransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountItemId",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_Unit_Quantity_UnitId",
                table: "IssuerTransaction");

            migrationBuilder.DropIndex(
                name: "IX_IssuerTransaction_IssuerAccountItemId",
                table: "IssuerTransaction");

            migrationBuilder.DropColumn(
                name: "MaxDurationBeforeValidityStart",
                table: "HolderTransactionRequest");

            migrationBuilder.DropColumn(
                name: "MinDurationBeforeValidityEnd",
                table: "HolderTransactionRequest");

            migrationBuilder.DropColumn(
                name: "MustBeExchangeable",
                table: "HolderTransactionRequest");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "DomainAccount");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitId",
                table: "IssuerTransaction",
                newName: "Quantity_UnitId1");

            migrationBuilder.RenameIndex(
                name: "IX_IssuerTransaction_Quantity_UnitId",
                table: "IssuerTransaction",
                newName: "IX_IssuerTransaction_Quantity_UnitId1");

            migrationBuilder.RenameColumn(
                name: "Quantity_UnitId",
                table: "HolderTransactionItem",
                newName: "Quantity_UnitId1");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionItem_Quantity_UnitId",
                table: "HolderTransactionItem",
                newName: "IX_HolderTransactionItem_Quantity_UnitId1");
            
            migrationBuilder.RenameColumn(
                name: "IssuerAccountItemId",
                table: "IssuerTransaction",
                newName: "IssuerAccountItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransaction_IssuerAccountItemId1",
                table: "IssuerTransaction",
                column: "IssuerAccountItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_Unit_Quantity_UnitId1",
                table: "HolderTransactionItem",
                column: "Quantity_UnitId1",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountItemId1",
                table: "IssuerTransaction",
                column: "IssuerAccountItemId1",
                principalTable: "AccountItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_Unit_Quantity_UnitId1",
                table: "IssuerTransaction",
                column: "Quantity_UnitId1",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
