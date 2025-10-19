using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PartManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddElectronicsMetadataAndNfcSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Datasheet",
                table: "Parts",
                newName: "QrCode");

            migrationBuilder.RenameColumn(
                name: "GridRow",
                table: "Drawers",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "GridColumn",
                table: "Drawers",
                newName: "GridY");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Parts",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Current",
                table: "Parts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footprint",
                table: "Parts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturerPartNumber",
                table: "Parts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinQuantity",
                table: "Parts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NfcTagId",
                table: "Parts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Parts",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "Parts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Power",
                table: "Parts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temperature",
                table: "Parts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tolerance",
                table: "Parts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Parts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voltage",
                table: "Parts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GridHeight",
                table: "Drawers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GridWidth",
                table: "Drawers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GridX",
                table: "Drawers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NfcTagId",
                table: "Drawers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "Drawers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PartAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FileUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartAttachments_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PartAttachments_PartId",
                table: "PartAttachments",
                column: "PartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_NfcTagId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_QrCode",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "Current",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Footprint",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "ManufacturerPartNumber",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "MinQuantity",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "NfcTagId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Tolerance",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Voltage",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "GridHeight",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "GridWidth",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "GridX",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "NfcTagId",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "Drawers");

            migrationBuilder.RenameColumn(
                name: "QrCode",
                table: "Parts",
                newName: "Datasheet");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Drawers",
                newName: "GridRow");

            migrationBuilder.RenameColumn(
                name: "GridY",
                table: "Drawers",
                newName: "GridColumn");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Parts",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
