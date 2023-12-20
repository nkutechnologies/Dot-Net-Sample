using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class addsponcertbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonorSponcers",
                columns: table => new
                {
                    SponcerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonateFrom = table.Column<DateTime>(nullable: false),
                    DonateTo = table.Column<DateTime>(nullable: false),
                    TotalFamilies = table.Column<int>(nullable: false),
                    EstimatedExpense = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalMeals = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    GroceryKitId = table.Column<int>(nullable: false),
                    DonorId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorSponcers", x => x.SponcerId);
                    table.ForeignKey(
                        name: "FK_DonorSponcers_AspNetUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonorSponcers_GroceryKits_GroceryKitId",
                        column: x => x.GroceryKitId,
                        principalTable: "GroceryKits",
                        principalColumn: "GroceryKitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonorSponcers_DonorId",
                table: "DonorSponcers",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorSponcers_GroceryKitId",
                table: "DonorSponcers",
                column: "GroceryKitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonorSponcers");
        }
    }
}
