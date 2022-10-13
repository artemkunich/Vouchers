using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Transactionandrequestmessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "HolderTransactionRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "HolderTransaction",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "HolderTransactionRequest");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "HolderTransaction");
        }
    }
}
