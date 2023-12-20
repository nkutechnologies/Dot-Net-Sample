using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class totalfamilymemberclm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalFamilyMember",
                table: "PackageDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalFamilyMember",
                table: "PackageDetails");
        }
    }
}
