using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.Persistence.Migrations
{
    public partial class DomainOfferAndContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "DomainOffer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OffersPerIdentityCounterId",
                table: "DomainContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_OffersPerIdentityCounterId",
                table: "DomainContract",
                column: "OffersPerIdentityCounterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_DomainOffersPerIdentityCounter_OffersPerIdentityCounterId",
                table: "DomainContract",
                column: "OffersPerIdentityCounterId",
                principalTable: "DomainOffersPerIdentityCounter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_DomainOffersPerIdentityCounter_OffersPerIdentityCounterId",
                table: "DomainContract");

            migrationBuilder.DropIndex(
                name: "IX_DomainContract_OffersPerIdentityCounterId",
                table: "DomainContract");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "OffersPerIdentityCounterId",
                table: "DomainContract");
        }
    }
}
