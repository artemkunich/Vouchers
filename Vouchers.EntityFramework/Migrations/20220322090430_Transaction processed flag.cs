using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Transactionprocessedflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_DomainAccount_CreditorId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_DomainAccount_DebtorId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_VoucherValue_Quantity_UnitId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Transaction_HolderTransactionId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Voucher_Quantity_UnitId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_VoucherAccount_CreditAccountId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_VoucherAccount_DebitAccountId",
                table: "TransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "TransactionItem",
                newName: "HolderTransactionItem");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "HolderTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_Quantity_UnitId",
                table: "HolderTransactionItem",
                newName: "IX_HolderTransactionItem_Quantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_HolderTransactionId",
                table: "HolderTransactionItem",
                newName: "IX_HolderTransactionItem_HolderTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_DebitAccountId",
                table: "HolderTransactionItem",
                newName: "IX_HolderTransactionItem_DebitAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_CreditAccountId",
                table: "HolderTransactionItem",
                newName: "IX_HolderTransactionItem_CreditAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_Quantity_UnitId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_Quantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_DebtorId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_DebtorId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CreditorId",
                table: "HolderTransaction",
                newName: "IX_HolderTransaction_CreditorId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPerformed",
                table: "HolderTransaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolderTransactionItem",
                table: "HolderTransactionItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolderTransaction",
                table: "HolderTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_DomainAccount_CreditorId",
                table: "HolderTransaction",
                column: "CreditorId",
                principalTable: "DomainAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_DomainAccount_DebtorId",
                table: "HolderTransaction",
                column: "DebtorId",
                principalTable: "DomainAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_VoucherValue_Quantity_UnitId",
                table: "HolderTransaction",
                column: "Quantity_UnitId",
                principalTable: "VoucherValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_HolderTransaction_HolderTransactionId",
                table: "HolderTransactionItem",
                column: "HolderTransactionId",
                principalTable: "HolderTransaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_Voucher_Quantity_UnitId",
                table: "HolderTransactionItem",
                column: "Quantity_UnitId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_VoucherAccount_CreditAccountId",
                table: "HolderTransactionItem",
                column: "CreditAccountId",
                principalTable: "VoucherAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_VoucherAccount_DebitAccountId",
                table: "HolderTransactionItem",
                column: "DebitAccountId",
                principalTable: "VoucherAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_DomainAccount_CreditorId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_DomainAccount_DebtorId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransaction_VoucherValue_Quantity_UnitId",
                table: "HolderTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_HolderTransaction_HolderTransactionId",
                table: "HolderTransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_Voucher_Quantity_UnitId",
                table: "HolderTransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_VoucherAccount_CreditAccountId",
                table: "HolderTransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderTransactionItem_VoucherAccount_DebitAccountId",
                table: "HolderTransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolderTransactionItem",
                table: "HolderTransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolderTransaction",
                table: "HolderTransaction");

            migrationBuilder.DropColumn(
                name: "IsPerformed",
                table: "HolderTransaction");

            migrationBuilder.RenameTable(
                name: "HolderTransactionItem",
                newName: "TransactionItem");

            migrationBuilder.RenameTable(
                name: "HolderTransaction",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionItem_Quantity_UnitId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_Quantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionItem_HolderTransactionId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_HolderTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionItem_DebitAccountId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_DebitAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransactionItem_CreditAccountId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_CreditAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_Quantity_UnitId",
                table: "Transaction",
                newName: "IX_Transaction_Quantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_DebtorId",
                table: "Transaction",
                newName: "IX_Transaction_DebtorId");

            migrationBuilder.RenameIndex(
                name: "IX_HolderTransaction_CreditorId",
                table: "Transaction",
                newName: "IX_Transaction_CreditorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_DomainAccount_CreditorId",
                table: "Transaction",
                column: "CreditorId",
                principalTable: "DomainAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_DomainAccount_DebtorId",
                table: "Transaction",
                column: "DebtorId",
                principalTable: "DomainAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_VoucherValue_Quantity_UnitId",
                table: "Transaction",
                column: "Quantity_UnitId",
                principalTable: "VoucherValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Transaction_HolderTransactionId",
                table: "TransactionItem",
                column: "HolderTransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Voucher_Quantity_UnitId",
                table: "TransactionItem",
                column: "Quantity_UnitId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_VoucherAccount_CreditAccountId",
                table: "TransactionItem",
                column: "CreditAccountId",
                principalTable: "VoucherAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_VoucherAccount_DebitAccountId",
                table: "TransactionItem",
                column: "DebitAccountId",
                principalTable: "VoucherAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
