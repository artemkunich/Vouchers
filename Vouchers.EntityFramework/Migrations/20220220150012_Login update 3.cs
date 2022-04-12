using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Loginupdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Login_IdentityId",
                table: "Login");

            migrationBuilder.CreateIndex(
                name: "IX_Login_IdentityId",
                table: "Login",
                column: "IdentityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Login_IdentityId",
                table: "Login");

            migrationBuilder.CreateIndex(
                name: "IX_Login_IdentityId",
                table: "Login",
                column: "IdentityId");
        }
    }
}
