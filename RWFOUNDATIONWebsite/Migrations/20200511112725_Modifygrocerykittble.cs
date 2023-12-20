using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class Modifygrocerykittble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "OtherDocumentAttaches");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUrl",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BFormUrl",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CNICBackUrl",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CNICFrontUrl",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeathCertificateUrl",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisabilityCertificateUrl",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ElectricityBill",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocument1Url",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocument2Url",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocumentName1",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocumentName2",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PtclBillUrl",
                table: "GroceryKits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUrl",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "BFormUrl",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "CNICBackUrl",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "CNICFrontUrl",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "DeathCertificateUrl",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "DisabilityCertificateUrl",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "ElectricityBill",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "OtherDocument1Url",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "OtherDocument2Url",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "OtherDocumentName1",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "OtherDocumentName2",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "PtclBillUrl",
                table: "GroceryKits");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroceryKitId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_GroceryKits_GroceryKitId",
                        column: x => x.GroceryKitId,
                        principalTable: "GroceryKits",
                        principalColumn: "GroceryKitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherDocumentAttaches",
                columns: table => new
                {
                    OtherDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroceryKitId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDocumentAttaches", x => x.OtherDocumentId);
                    table.ForeignKey(
                        name: "FK_OtherDocumentAttaches_GroceryKits_GroceryKitId",
                        column: x => x.GroceryKitId,
                        principalTable: "GroceryKits",
                        principalColumn: "GroceryKitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_GroceryKitId",
                table: "Documents",
                column: "GroceryKitId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherDocumentAttaches_GroceryKitId",
                table: "OtherDocumentAttaches",
                column: "GroceryKitId");
        }
    }
}
