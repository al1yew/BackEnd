using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class AddedPositionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Trainers");

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Trainers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_PositionId",
                table: "Trainers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Positions_PositionId",
                table: "Trainers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Positions_PositionId",
                table: "Trainers");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_PositionId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Trainers");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
