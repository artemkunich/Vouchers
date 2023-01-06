using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vouchers.Persistence.Migrations
{
    public partial class Croppedimages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppImage",
                table: "AppImage");

            migrationBuilder.DropColumn(
                name: "CroppedContent",
                table: "AppImage");

            migrationBuilder.RenameTable(
                name: "AppImage",
                newName: "CroppedImage");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "CroppedImage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CroppedImage",
                table: "CroppedImage",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CroppedImage",
                table: "CroppedImage");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "CroppedImage");

            migrationBuilder.RenameTable(
                name: "CroppedImage",
                newName: "AppImage");

            migrationBuilder.AddColumn<byte[]>(
                name: "CroppedContent",
                table: "AppImage",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppImage",
                table: "AppImage",
                column: "Id");
        }
    }
}
