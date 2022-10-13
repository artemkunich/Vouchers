using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Accountcolumnnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountItem_Account_HolderId",
                table: "AccountItem");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_Account_CreditorId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_Account_DebtorId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_Account_CreditorId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_Account_DebtorId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountId1",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitType_Account_IssuerId",
                table: "UnitType");

            migrationBuilder.RenameColumn(
                name: "IssuerId",
                table: "UnitType",
                newName: "IssuerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitType_IssuerId",
                table: "UnitType",
                newName: "IX_UnitType_IssuerAccountId");

            migrationBuilder.RenameColumn(
                name: "IssuerAccountId1",
                table: "IssuerTransaction",
                newName: "IssuerAccountItemId1");

            migrationBuilder.RenameIndex(
                name: "IX_IssuerTransaction_IssuerAccountId1",
                table: "IssuerTransaction",
                newName: "IX_IssuerTransaction_IssuerAccountItemId1");

            migrationBuilder.RenameColumn(
                name: "DebtorId",
                table: "HolderTransactionRequest",
                newName: "DebtorAccountId");

            migrationBuilder.RenameColumn(
                name: "CreditorId",
                table: "HolderTransactionRequest",
                newName: "CreditorAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionRequest_DebtorId",
                table: "HolderTransactionRequest",
                newName: "IX_HolderTransactionRequest_DebtorAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionRequest_CreditorId",
                table: "HolderTransactionRequest",
                newName: "IX_HolderTransactionRequest_CreditorAccountId");

            migrationBuilder.RenameColumn(
                name: "DebtorId",
                table: "HolderTransaction",
                newName: "DebtorAccountId");

            migrationBuilder.RenameColumn(
                name: "CreditorId",
                table: "HolderTransaction",
                newName: "CreditorAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_DebtorId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_DebtorAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_CreditorId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_CreditorAccountId");

            migrationBuilder.RenameColumn(
                name: "HolderId",
                table: "AccountItem",
                newName: "HolderAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountItem_HolderId_UnitId",
                table: "AccountItem",
                newName: "IX_AccountItem_HolderAccountId_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountItem_Account_HolderAccountId",
                table: "AccountItem",
                column: "HolderAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_Account_CreditorAccountId",
                table: "HolderTransaction",
                column: "CreditorAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_Account_DebtorAccountId",
                table: "HolderTransaction",
                column: "DebtorAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_Account_CreditorAccountId",
                table: "HolderTransactionRequest",
                column: "CreditorAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_Account_DebtorAccountId",
                table: "HolderTransactionRequest",
                column: "DebtorAccountId",
                principalTable: "Account",
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
                name: "FK_UnitType_Account_IssuerAccountId",
                table: "UnitType",
                column: "IssuerAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountItem_Account_HolderAccountId",
                table: "AccountItem");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_Account_CreditorAccountId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_Account_DebtorAccountId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_Account_CreditorAccountId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionRequest_Account_DebtorAccountId",
                table: "HolderTransactionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountItemId1",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitType_Account_IssuerAccountId",
                table: "UnitType");

            migrationBuilder.RenameColumn(
                name: "IssuerAccountId",
                table: "UnitType",
                newName: "IssuerId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitType_IssuerAccountId",
                table: "UnitType",
                newName: "IX_UnitType_IssuerId");

            migrationBuilder.RenameColumn(
                name: "IssuerAccountItemId1",
                table: "IssuerTransaction",
                newName: "IssuerAccountId1");

            migrationBuilder.RenameIndex(
                name: "IX_IssuerTransaction_IssuerAccountItemId1",
                table: "IssuerTransaction",
                newName: "IX_IssuerTransaction_IssuerAccountId1");

            migrationBuilder.RenameColumn(
                name: "DebtorAccountId",
                table: "HolderTransactionRequest",
                newName: "DebtorId");

            migrationBuilder.RenameColumn(
                name: "CreditorAccountId",
                table: "HolderTransactionRequest",
                newName: "CreditorId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionRequest_DebtorAccountId",
                table: "HolderTransactionRequest",
                newName: "IX_HolderTransactionRequest_DebtorId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionRequest_CreditorAccountId",
                table: "HolderTransactionRequest",
                newName: "IX_HolderTransactionRequest_CreditorId");

            migrationBuilder.RenameColumn(
                name: "DebtorAccountId",
                table: "HolderTransaction",
                newName: "DebtorId");

            migrationBuilder.RenameColumn(
                name: "CreditorAccountId",
                table: "HolderTransaction",
                newName: "CreditorId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_DebtorAccountId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_DebtorId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_CreditorAccountId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_CreditorId");

            migrationBuilder.RenameColumn(
                name: "HolderAccountId",
                table: "AccountItem",
                newName: "HolderId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountItem_HolderAccountId_UnitId",
                table: "AccountItem",
                newName: "IX_AccountItem_HolderId_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountItem_Account_HolderId",
                table: "AccountItem",
                column: "HolderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_Account_CreditorId",
                table: "HolderTransaction",
                column: "CreditorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_Account_DebtorId",
                table: "HolderTransaction",
                column: "DebtorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_Account_CreditorId",
                table: "HolderTransactionRequest",
                column: "CreditorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionRequest_Account_DebtorId",
                table: "HolderTransactionRequest",
                column: "DebtorId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_AccountItem_IssuerAccountId1",
                table: "IssuerTransaction",
                column: "IssuerAccountId1",
                principalTable: "AccountItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitType_Account_IssuerId",
                table: "UnitType",
                column: "IssuerId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
