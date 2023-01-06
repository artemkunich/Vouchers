using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vouchers.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConsumedMessageUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConsumedMessage_MessageId_Consumer",
                table: "ConsumedMessage");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedMessage_MessageId_Consumer",
                table: "ConsumedMessage",
                columns: new[] { "MessageId", "Consumer" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConsumedMessage_MessageId_Consumer",
                table: "ConsumedMessage");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedMessage_MessageId_Consumer",
                table: "ConsumedMessage",
                columns: new[] { "MessageId", "Consumer" });
        }
    }
}
