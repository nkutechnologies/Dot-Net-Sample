using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class Addfamilymemberfordonortbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyMember",
                table: "DonorRequestForBeneficiaries");

            migrationBuilder.CreateTable(
                name: "FamilyMemberForDonors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorRequestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMemberForDonors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyMemberForDonors_DonorRequestForBeneficiaries_DonorRequestId",
                        column: x => x.DonorRequestId,
                        principalTable: "DonorRequestForBeneficiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMemberForDonors_DonorRequestId",
                table: "FamilyMemberForDonors",
                column: "DonorRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyMemberForDonors");

            migrationBuilder.AddColumn<int>(
                name: "FamilyMember",
                table: "DonorRequestForBeneficiaries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
