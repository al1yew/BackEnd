using Microsoft.EntityFrameworkCore.Migrations;

namespace Allup.Migrations
{
    public partial class Products_Extax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExTax",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExTax",
                table: "Products",
                type: "money",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
