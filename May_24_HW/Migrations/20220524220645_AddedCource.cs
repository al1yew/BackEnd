using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class AddedCource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    TextContent = table.Column<string>(nullable: true),
                    SmallImage = table.Column<string>(nullable: true),
                    Liked = table.Column<int>(nullable: false),
                    Users = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cources", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cources");
        }
    }
}
