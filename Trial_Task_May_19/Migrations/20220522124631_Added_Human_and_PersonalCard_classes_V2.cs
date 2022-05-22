using Microsoft.EntityFrameworkCore.Migrations;

namespace Trial_Task_May_19.Migrations
{
    public partial class Added_Human_and_PersonalCard_classes_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PIN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Humen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PersonalCardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Humen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Humen_PersonalCards_PersonalCardId",
                        column: x => x.PersonalCardId,
                        principalTable: "PersonalCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Humen_PersonalCardId",
                table: "Humen",
                column: "PersonalCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Humen");

            migrationBuilder.DropTable(
                name: "PersonalCards");
        }
    }
}
