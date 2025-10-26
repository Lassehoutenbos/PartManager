using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixPostgreSQLFilterSyntax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drawers_NfcTagId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_QrCode",
                table: "Drawers");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_NfcTagId",
                table: "Drawers",
                column: "NfcTagId",
                unique: true,
                filter: "\"NfcTagId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_QrCode",
                table: "Drawers",
                column: "QrCode",
                unique: true,
                filter: "\"QrCode\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drawers_NfcTagId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_QrCode",
                table: "Drawers");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_NfcTagId",
                table: "Drawers",
                column: "NfcTagId",
                unique: true,
                filter: "[NfcTagId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_QrCode",
                table: "Drawers",
                column: "QrCode",
                unique: true,
                filter: "[QrCode] IS NOT NULL");
        }
    }
}
