using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class DomainOfferMaxMembersCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxSubscribersCount",
                table: "DomainOffer",
                newName: "MaxMembersCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxMembersCount",
                table: "DomainOffer",
                newName: "MaxSubscribersCount");
        }
    }
}
