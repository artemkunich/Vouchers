using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Loginupdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValueDetail_Identity_VoucherValueId",
                table: "VoucherValueDetail");

            migrationBuilder.DropIndex(
                name: "IX_Login_IdentityId",
                table: "Login");

            migrationBuilder.RenameColumn(
                name: "VoucherValueId",
                table: "VoucherValueDetail",
                newName: "ValueId");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherValueDetail_VoucherValueId",
                table: "VoucherValueDetail",
                newName: "IX_VoucherValueDetail_ValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Login_IdentityId",
                table: "Login",
                column: "IdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValueDetail_Identity_ValueId",
                table: "VoucherValueDetail",
                column: "ValueId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherValueDetail_Identity_ValueId",
                table: "VoucherValueDetail");

            migrationBuilder.DropIndex(
                name: "IX_Login_IdentityId",
                table: "Login");

            migrationBuilder.RenameColumn(
                name: "ValueId",
                table: "VoucherValueDetail",
                newName: "VoucherValueId");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherValueDetail_ValueId",
                table: "VoucherValueDetail",
                newName: "IX_VoucherValueDetail_VoucherValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Login_IdentityId",
                table: "Login",
                column: "IdentityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherValueDetail_Identity_VoucherValueId",
                table: "VoucherValueDetail",
                column: "VoucherValueId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
