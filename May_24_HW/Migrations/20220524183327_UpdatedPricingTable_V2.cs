using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class UpdatedPricingTable_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "Pricings");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Pricings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Pricings");

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "Pricings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
