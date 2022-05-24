using Microsoft.EntityFrameworkCore.Migrations;

namespace May_24_HW.Migrations
{
    public partial class UpdatedTrainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Positions_PositionId",
                table: "Trainers");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "Trainers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Positions_PositionId",
                table: "Trainers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Positions_PositionId",
                table: "Trainers");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "Trainers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Positions_PositionId",
                table: "Trainers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
