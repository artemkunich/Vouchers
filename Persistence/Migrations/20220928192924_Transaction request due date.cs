using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.Persistence.Migrations
{
    public partial class Transactionrequestduedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "HolderTransactionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "HolderTransactionRequest");
        }
    }
}
