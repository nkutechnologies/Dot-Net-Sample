using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class groceryassigntbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignTo",
                table: "GroceryKits");

            migrationBuilder.CreateTable(
                name: "GroceryKitAssigns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroceryKitId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryKitAssigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroceryKitAssigns_GroceryKits_GroceryKitId",
                        column: x => x.GroceryKitId,
                        principalTable: "GroceryKits",
                        principalColumn: "GroceryKitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroceryKitAssigns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryKitAssigns_GroceryKitId",
                table: "GroceryKitAssigns",
                column: "GroceryKitId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryKitAssigns_UserId",
                table: "GroceryKitAssigns",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroceryKitAssigns");

            migrationBuilder.AddColumn<int>(
                name: "AssignTo",
                table: "GroceryKits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
