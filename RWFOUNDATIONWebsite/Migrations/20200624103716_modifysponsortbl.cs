using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RWFOUNDATIONWebsite.Migrations
{
    public partial class modifysponsortbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonateFrom",
                table: "DonorSponcers");

            migrationBuilder.DropColumn(
                name: "DonateTo",
                table: "DonorSponcers");

            migrationBuilder.AddColumn<string>(
                name: "SponsorStatus",
                table: "GroceryKits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfMonth",
                table: "DonorSponcers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SponsorStatus",
                table: "GroceryKits");

            migrationBuilder.DropColumn(
                name: "NoOfMonth",
                table: "DonorSponcers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DonateFrom",
                table: "DonorSponcers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DonateTo",
                table: "DonorSponcers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
