using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class AddSaveAsDraftTbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeneficiaryFormSaveAsDrafts",
                columns: table => new
                {
                    GroceryKitId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormNo = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    NameUrdu = table.Column<string>(nullable: true),
                    FatherOrHusbandName = table.Column<string>(nullable: true),
                    FatherOrHusbandNameUrdu = table.Column<string>(nullable: true),
                    CNIC = table.Column<string>(nullable: true),
                    OldFormNo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    ProvinceId = table.Column<int>(nullable: false),
                    MedicalProbId = table.Column<int>(nullable: false),
                    OccupationId = table.Column<int>(nullable: false),
                    PhoneNumber1 = table.Column<string>(nullable: true),
                    PhoneNumber2 = table.Column<string>(nullable: true),
                    MeritalStatus = table.Column<string>(nullable: true),
                    CurrentStatusId = table.Column<int>(nullable: false),
                    ApplicationDate = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    ResidenceStatus = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    FoodCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HouseRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SchoolCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UtilitiesCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MedicalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ZakatAcceptable = table.Column<string>(nullable: true),
                    FamilySize = table.Column<int>(nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Donations = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShortFallInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    CNICFrontUrl = table.Column<string>(nullable: true),
                    CNICBackUrl = table.Column<string>(nullable: true),
                    DisabilityCertificateUrl = table.Column<string>(nullable: true),
                    DeathCertificateUrl = table.Column<string>(nullable: true),
                    BFormUrl = table.Column<string>(nullable: true),
                    ElectricityBill = table.Column<string>(nullable: true),
                    PtclBillUrl = table.Column<string>(nullable: true),
                    ApplicationUrl = table.Column<string>(nullable: true),
                    OtherDocumentName1 = table.Column<string>(nullable: true),
                    OtherDocumentName2 = table.Column<string>(nullable: true),
                    OtherDocument1Url = table.Column<string>(nullable: true),
                    OtherDocument2Url = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true),
                    AudioUrl = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiaryFormSaveAsDrafts", x => x.GroceryKitId);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFormSaveAsDrafts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFormSaveAsDrafts_CurrentStatuses_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "CurrentStatuses",
                        principalColumn: "CurrentStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFormSaveAsDrafts_MedicalProbs_MedicalProbId",
                        column: x => x.MedicalProbId,
                        principalTable: "MedicalProbs",
                        principalColumn: "MedicalProbId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFormSaveAsDrafts_Occupations_OccupationId",
                        column: x => x.OccupationId,
                        principalTable: "Occupations",
                        principalColumn: "OccupationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFormSaveAsDrafts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BeneficiaryFamilyMemberSaveAsDrafts",
                columns: table => new
                {
                    FamilyMemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    RelationId = table.Column<int>(nullable: false),
                    GroceryKitId = table.Column<int>(nullable: false),
                    FamilyMemberStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiaryFamilyMemberSaveAsDrafts", x => x.FamilyMemberId);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFamilyMemberSaveAsDrafts_FamilyMemberStatuses_FamilyMemberStatusId",
                        column: x => x.FamilyMemberStatusId,
                        principalTable: "FamilyMemberStatuses",
                        principalColumn: "FamilyMemberStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFamilyMemberSaveAsDrafts_BeneficiaryFormSaveAsDrafts_GroceryKitId",
                        column: x => x.GroceryKitId,
                        principalTable: "BeneficiaryFormSaveAsDrafts",
                        principalColumn: "GroceryKitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeneficiaryFamilyMemberSaveAsDrafts_Relations_RelationId",
                        column: x => x.RelationId,
                        principalTable: "Relations",
                        principalColumn: "RelationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFamilyMemberSaveAsDrafts_FamilyMemberStatusId",
                table: "BeneficiaryFamilyMemberSaveAsDrafts",
                column: "FamilyMemberStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFamilyMemberSaveAsDrafts_GroceryKitId",
                table: "BeneficiaryFamilyMemberSaveAsDrafts",
                column: "GroceryKitId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFamilyMemberSaveAsDrafts_RelationId",
                table: "BeneficiaryFamilyMemberSaveAsDrafts",
                column: "RelationId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFormSaveAsDrafts_CityId",
                table: "BeneficiaryFormSaveAsDrafts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFormSaveAsDrafts_CurrentStatusId",
                table: "BeneficiaryFormSaveAsDrafts",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFormSaveAsDrafts_MedicalProbId",
                table: "BeneficiaryFormSaveAsDrafts",
                column: "MedicalProbId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFormSaveAsDrafts_OccupationId",
                table: "BeneficiaryFormSaveAsDrafts",
                column: "OccupationId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryFormSaveAsDrafts_ProvinceId",
                table: "BeneficiaryFormSaveAsDrafts",
                column: "ProvinceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeneficiaryFamilyMemberSaveAsDrafts");

            migrationBuilder.DropTable(
                name: "BeneficiaryFormSaveAsDrafts");
        }
    }
}
