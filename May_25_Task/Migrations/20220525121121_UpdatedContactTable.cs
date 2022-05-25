using Microsoft.EntityFrameworkCore.Migrations;

namespace May_25_Task.Migrations
{
    public partial class UpdatedContactTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Contacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Contacts");
        }
    }
}
