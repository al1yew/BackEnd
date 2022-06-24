using Microsoft.EntityFrameworkCore.Migrations;

namespace Allup.Migrations
{
    public partial class Products_Extax_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ExTax",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExTax",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
