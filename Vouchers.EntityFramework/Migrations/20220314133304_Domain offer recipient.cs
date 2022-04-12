using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Domainofferrecipient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "DomainOffer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DomainOffer_RecipientId",
                table: "DomainOffer",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainOffer_Identity_RecipientId",
                table: "DomainOffer",
                column: "RecipientId",
                principalTable: "Identity",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainOffer_Identity_RecipientId",
                table: "DomainOffer");

            migrationBuilder.DropIndex(
                name: "IX_DomainOffer_RecipientId",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "DomainOffer");
        }
    }
}
