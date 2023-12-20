using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class modifydonorrequesttbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "DonorRequestForBeneficiaries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequestTo",
                table: "DonorRequestForBeneficiaries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "DonorRequestForBeneficiaries");

            migrationBuilder.DropColumn(
                name: "RequestTo",
                table: "DonorRequestForBeneficiaries");
        }
    }
}
