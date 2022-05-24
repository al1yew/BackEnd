using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class Updated_Pricing_Plan_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                table: "Pricings");

            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                table: "Plans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PricingPlanId",
                table: "Pricings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricingPlanId",
                table: "Plans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
