using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.EntityFramework.Migrations
{
    public partial class Domaincontracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainDetail_Domain_DomainId",
                table: "DomainDetail");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DomainOffer");

            migrationBuilder.RenameColumn(
                name: "Period",
                table: "DomainOffer",
                newName: "InvoicePeriod");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "DomainOffer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "DomainOffer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "DomainDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DomainContract",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainContract_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainContract_DomainOffer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "DomainOffer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainDetail_ContractId",
                table: "DomainDetail",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_DomainId",
                table: "DomainContract",
                column: "DomainId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainContract_OfferId",
                table: "DomainContract",
                column: "OfferId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainDetail_DomainContract_ContractId",
                table: "DomainDetail",
                column: "ContractId",
                principalTable: "DomainContract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainDetail_DomainContract_ContractId",
                table: "DomainDetail");

            migrationBuilder.DropTable(
                name: "DomainContract");

            migrationBuilder.DropIndex(
                name: "IX_DomainDetail_ContractId",
                table: "DomainDetail");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "DomainOffer");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "DomainDetail");

            migrationBuilder.RenameColumn(
                name: "InvoicePeriod",
                table: "DomainOffer",
                newName: "Period");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DomainOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainDetail_Domain_DomainId",
                table: "DomainDetail",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
