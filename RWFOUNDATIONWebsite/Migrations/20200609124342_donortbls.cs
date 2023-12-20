using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class donortbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonorSponcers_AspNetUsers_DonorId",
                table: "DonorSponcers");

            migrationBuilder.DropForeignKey(
                name: "FK_DonorSponcers_GroceryKits_GroceryKitId",
                table: "DonorSponcers");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_FamilyMemberStatuses_FamilyMemberStatusId",
                table: "FamilyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_Relations_RelationId",
                table: "FamilyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKitAssigns_GroceryKits_GroceryKitId",
                table: "GroceryKitAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKitAssigns_AspNetUsers_UserId",
                table: "GroceryKitAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_Cities_CityId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_CurrentStatuses_CurrentStatusId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_MedicalProbs_MedicalProbId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_Occupations_OccupationId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_Provinces_ProvinceId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageDetails_Packages_PackageId",
                table: "PackageDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageItems_Items_ItemId",
                table: "PackageItems");

            migrationBuilder.CreateTable(
                name: "DonationTypes",
                columns: table => new
                {
                    DonationTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationTypes", x => x.DonationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DonorRequestForBeneficiaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationTypeId = table.Column<int>(nullable: false),
                    DonorId = table.Column<int>(nullable: false),
                    ExpectedDonation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BeneficiaryType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorRequestForBeneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonorRequestForBeneficiaries_DonationTypes_DonationTypeId",
                        column: x => x.DonationTypeId,
                        principalTable: "DonationTypes",
                        principalColumn: "DonationTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonorRequestForBeneficiaries_AspNetUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonorRequestForBeneficiaries_DonationTypeId",
                table: "DonorRequestForBeneficiaries",
                column: "DonationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorRequestForBeneficiaries_DonorId",
                table: "DonorRequestForBeneficiaries",
                column: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonorSponcers_AspNetUsers_DonorId",
                table: "DonorSponcers",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonorSponcers_GroceryKits_GroceryKitId",
                table: "DonorSponcers",
                column: "GroceryKitId",
                principalTable: "GroceryKits",
                principalColumn: "GroceryKitId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_FamilyMemberStatuses_FamilyMemberStatusId",
                table: "FamilyMembers",
                column: "FamilyMemberStatusId",
                principalTable: "FamilyMemberStatuses",
                principalColumn: "FamilyMemberStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_Relations_RelationId",
                table: "FamilyMembers",
                column: "RelationId",
                principalTable: "Relations",
                principalColumn: "RelationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKitAssigns_GroceryKits_GroceryKitId",
                table: "GroceryKitAssigns",
                column: "GroceryKitId",
                principalTable: "GroceryKits",
                principalColumn: "GroceryKitId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKitAssigns_AspNetUsers_UserId",
                table: "GroceryKitAssigns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_Cities_CityId",
                table: "GroceryKits",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_CurrentStatuses_CurrentStatusId",
                table: "GroceryKits",
                column: "CurrentStatusId",
                principalTable: "CurrentStatuses",
                principalColumn: "CurrentStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_MedicalProbs_MedicalProbId",
                table: "GroceryKits",
                column: "MedicalProbId",
                principalTable: "MedicalProbs",
                principalColumn: "MedicalProbId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_Occupations_OccupationId",
                table: "GroceryKits",
                column: "OccupationId",
                principalTable: "Occupations",
                principalColumn: "OccupationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_Provinces_ProvinceId",
                table: "GroceryKits",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "ProvinceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageDetails_Packages_PackageId",
                table: "PackageDetails",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "PackageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageItems_Items_ItemId",
                table: "PackageItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonorSponcers_AspNetUsers_DonorId",
                table: "DonorSponcers");

            migrationBuilder.DropForeignKey(
                name: "FK_DonorSponcers_GroceryKits_GroceryKitId",
                table: "DonorSponcers");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_FamilyMemberStatuses_FamilyMemberStatusId",
                table: "FamilyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_Relations_RelationId",
                table: "FamilyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKitAssigns_GroceryKits_GroceryKitId",
                table: "GroceryKitAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKitAssigns_AspNetUsers_UserId",
                table: "GroceryKitAssigns");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_Cities_CityId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_CurrentStatuses_CurrentStatusId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_MedicalProbs_MedicalProbId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_Occupations_OccupationId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryKits_Provinces_ProvinceId",
                table: "GroceryKits");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageDetails_Packages_PackageId",
                table: "PackageDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageItems_Items_ItemId",
                table: "PackageItems");

            migrationBuilder.DropTable(
                name: "DonorRequestForBeneficiaries");

            migrationBuilder.DropTable(
                name: "DonationTypes");

            migrationBuilder.AddForeignKey(
                name: "FK_DonorSponcers_AspNetUsers_DonorId",
                table: "DonorSponcers",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonorSponcers_GroceryKits_GroceryKitId",
                table: "DonorSponcers",
                column: "GroceryKitId",
                principalTable: "GroceryKits",
                principalColumn: "GroceryKitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_FamilyMemberStatuses_FamilyMemberStatusId",
                table: "FamilyMembers",
                column: "FamilyMemberStatusId",
                principalTable: "FamilyMemberStatuses",
                principalColumn: "FamilyMemberStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_Relations_RelationId",
                table: "FamilyMembers",
                column: "RelationId",
                principalTable: "Relations",
                principalColumn: "RelationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKitAssigns_GroceryKits_GroceryKitId",
                table: "GroceryKitAssigns",
                column: "GroceryKitId",
                principalTable: "GroceryKits",
                principalColumn: "GroceryKitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKitAssigns_AspNetUsers_UserId",
                table: "GroceryKitAssigns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_Cities_CityId",
                table: "GroceryKits",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_CurrentStatuses_CurrentStatusId",
                table: "GroceryKits",
                column: "CurrentStatusId",
                principalTable: "CurrentStatuses",
                principalColumn: "CurrentStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_MedicalProbs_MedicalProbId",
                table: "GroceryKits",
                column: "MedicalProbId",
                principalTable: "MedicalProbs",
                principalColumn: "MedicalProbId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_Occupations_OccupationId",
                table: "GroceryKits",
                column: "OccupationId",
                principalTable: "Occupations",
                principalColumn: "OccupationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryKits_Provinces_ProvinceId",
                table: "GroceryKits",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "ProvinceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageDetails_Packages_PackageId",
                table: "PackageDetails",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "PackageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageItems_Items_ItemId",
                table: "PackageItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
