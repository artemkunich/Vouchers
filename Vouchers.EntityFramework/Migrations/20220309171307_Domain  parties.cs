using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Domainparties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartyId",
                table: "DomainContract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_PartyId",
                table: "DomainContract",
                column: "PartyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainContract_Identity_PartyId",
                table: "DomainContract",
                column: "PartyId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainContract_Identity_PartyId",
                table: "DomainContract");

            migrationBuilder.DropIndex(
                name: "IX_DomainContract_PartyId",
                table: "DomainContract");

            migrationBuilder.DropColumn(
                name: "PartyId",
                table: "DomainContract");
        }
    }
}
