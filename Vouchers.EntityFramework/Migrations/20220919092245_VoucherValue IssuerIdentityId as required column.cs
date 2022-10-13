using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class VoucherValueIssuerIdentityIdasrequiredcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IssuerIdentityId",
                table: "VoucherValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuerIdentityId",
                table: "VoucherValue");
        }
    }
}
