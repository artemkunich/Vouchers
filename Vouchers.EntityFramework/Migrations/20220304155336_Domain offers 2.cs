using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Domainoffers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount_Amount",
                table: "DomainOffer",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Amount_Currency",
                table: "DomainOffer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxSubscribersCount",
                table: "DomainOffer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DomainOffer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "DomainOffer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount_Amount",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "Amount_Currency",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "MaxSubscribersCount",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "DomainOffer");
        }
    }
}
