using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class Updated_Pricing_removedFromPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLined",
                table: "Plans");

            migrationBuilder.AddColumn<bool>(
                name: "IsLined",
                table: "Pricings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLined",
                table: "Pricings");

            migrationBuilder.AddColumn<bool>(
                name: "IsLined",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
