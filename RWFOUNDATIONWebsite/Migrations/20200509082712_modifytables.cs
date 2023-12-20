using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class modifytables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldFormNo",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Cities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                column: "ProvinceId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Cities_Provinces_ProvinceId",
            //    table: "Cities",
            //    column: "ProvinceId",
            //    principalTable: "Provinces",
            //    principalColumn: "ProvinceId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "OldFormNo",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Cities");
        }
    }
}
