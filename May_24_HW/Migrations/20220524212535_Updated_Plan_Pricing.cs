using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class Updated_Plan_Pricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PricingPlanId",
                table: "Pricings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricingPlanId",
                table: "Plans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                table: "Pricings");

            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                table: "Plans");
        }
    }
}
