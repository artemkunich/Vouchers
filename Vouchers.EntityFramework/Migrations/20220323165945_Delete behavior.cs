using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Deletebehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainAccount_Domain_DomainId",
                table: "DomainAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainAccount_Identity_IdentityId",
                table: "DomainAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_Domain_DomainId",
                table: "DomainContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_DomainOffer_OfferId",
                table: "DomainContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_Identity_PartyId",
                table: "DomainContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainOffer_Identity_RecipientId",
                table: "DomainOffer");

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
                name: "FK_IdentityDetail_Identity_IdentityId",
                table: "IdentityDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_Voucher_Quantity_UnitId",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_VoucherAccount_IssuerAccountId",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Login_Identity_IdentityId",
                table: "Login");

            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_VoucherValue_ValueId",
                table: "Voucher");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherAccount_DomainAccount_HolderId",
                table: "VoucherAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherAccount_Voucher_UnitId",
                table: "VoucherAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValue_DomainAccount_IssuerId",
                table: "VoucherValue");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValueDetail_Domain_DomainId",
                table: "VoucherValueDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValueDetail_Identity_ValueId",
                table: "VoucherValueDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainAccount_Domain_DomainId",
                table: "DomainAccount",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainAccount_Identity_IdentityId",
                table: "DomainAccount",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_Domain_DomainId",
                table: "DomainContract",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_DomainOffer_OfferId",
                table: "DomainContract",
                column: "OfferId",
                principalTable: "DomainOffer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_Identity_PartyId",
                table: "DomainContract",
                column: "PartyId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainOffer_Identity_RecipientId",
                table: "DomainOffer",
                column: "RecipientId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_DomainAccount_CreditorId",
                table: "HolderTransaction",
                column: "CreditorId",
                principalTable: "DomainAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_DomainAccount_DebtorId",
                table: "HolderTransaction",
                column: "DebtorId",
                principalTable: "DomainAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransaction_VoucherValue_Quantity_UnitId",
                table: "HolderTransaction",
                column: "Quantity_UnitId",
                principalTable: "VoucherValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderTransactionItem_HolderTransaction_HolderTransactionId",
                table: "HolderTransactionItem",
                column: "HolderTransactionId",
                principalTable: "HolderTransaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityDetail_Identity_IdentityId",
                table: "IdentityDetail",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_Voucher_Quantity_UnitId",
                table: "IssuerTransaction",
                column: "Quantity_UnitId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_VoucherAccount_IssuerAccountId",
                table: "IssuerTransaction",
                column: "IssuerAccountId",
                principalTable: "VoucherAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Login_Identity_IdentityId",
                table: "Login",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_VoucherValue_ValueId",
                table: "Voucher",
                column: "ValueId",
                principalTable: "VoucherValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherAccount_DomainAccount_HolderId",
                table: "VoucherAccount",
                column: "HolderId",
                principalTable: "DomainAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherAccount_Voucher_UnitId",
                table: "VoucherAccount",
                column: "UnitId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValue_DomainAccount_IssuerId",
                table: "VoucherValue",
                column: "IssuerId",
                principalTable: "DomainAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValueDetail_Domain_DomainId",
                table: "VoucherValueDetail",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValueDetail_VoucherValue_ValueId",
                table: "VoucherValueDetail",
                column: "ValueId",
                principalTable: "VoucherValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainAccount_Domain_DomainId",
                table: "DomainAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainAccount_Identity_IdentityId",
                table: "DomainAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_Domain_DomainId",
                table: "DomainContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_DomainOffer_OfferId",
                table: "DomainContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_Identity_PartyId",
                table: "DomainContract");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainOffer_Identity_RecipientId",
                table: "DomainOffer");

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
                name: "FK_IdentityDetail_Identity_IdentityId",
                table: "IdentityDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_Voucher_Quantity_UnitId",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuerTransaction_VoucherAccount_IssuerAccountId",
                table: "IssuerTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Login_Identity_IdentityId",
                table: "Login");

            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_VoucherValue_ValueId",
                table: "Voucher");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherAccount_DomainAccount_HolderId",
                table: "VoucherAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherAccount_Voucher_UnitId",
                table: "VoucherAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValue_DomainAccount_IssuerId",
                table: "VoucherValue");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValueDetail_Domain_DomainId",
                table: "VoucherValueDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValueDetail_VoucherValue_ValueId",
                table: "VoucherValueDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainAccount_Domain_DomainId",
                table: "DomainAccount",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainAccount_Identity_IdentityId",
                table: "DomainAccount",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_Domain_DomainId",
                table: "DomainContract",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_DomainOffer_OfferId",
                table: "DomainContract",
                column: "OfferId",
                principalTable: "DomainOffer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_Identity_PartyId",
                table: "DomainContract",
                column: "PartyId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainOffer_Identity_RecipientId",
                table: "DomainOffer",
                column: "RecipientId",
                principalTable: "Identity",
                principalColumn: "Id");

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
                name: "FK_IdentityDetail_Identity_IdentityId",
                table: "IdentityDetail",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_Voucher_Quantity_UnitId",
                table: "IssuerTransaction",
                column: "Quantity_UnitId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuerTransaction_VoucherAccount_IssuerAccountId",
                table: "IssuerTransaction",
                column: "IssuerAccountId",
                principalTable: "VoucherAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Login_Identity_IdentityId",
                table: "Login",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_VoucherValue_ValueId",
                table: "Voucher",
                column: "ValueId",
                principalTable: "VoucherValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherAccount_DomainAccount_HolderId",
                table: "VoucherAccount",
                column: "HolderId",
                principalTable: "DomainAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherAccount_Voucher_UnitId",
                table: "VoucherAccount",
                column: "UnitId",
                principalTable: "Voucher",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValue_DomainAccount_IssuerId",
                table: "VoucherValue",
                column: "IssuerId",
                principalTable: "DomainAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValueDetail_Domain_DomainId",
                table: "VoucherValueDetail",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValueDetail_Identity_ValueId",
                table: "VoucherValueDetail",
                column: "ValueId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
