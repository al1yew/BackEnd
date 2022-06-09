using Microsoft.EntityFrameworkCore.Migrations;

namespace Allup.Migrations
{
    public partial class AddedProductImageTableRelatedProductsTabe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductToTag_Products_ProductId",
                table: "ProductToTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductToTag_Tags_TagId",
                table: "ProductToTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductToTag",
                table: "ProductToTag");

            migrationBuilder.RenameTable(
                name: "ProductToTag",
                newName: "ProductToTags");

            migrationBuilder.RenameIndex(
                name: "IX_ProductToTag_TagId",
                table: "ProductToTags",
                newName: "IX_ProductToTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductToTag_ProductId",
                table: "ProductToTags",
                newName: "IX_ProductToTags_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductToTags",
                table: "ProductToTags",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductToTags_Products_ProductId",
                table: "ProductToTags",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductToTags_Tags_TagId",
                table: "ProductToTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductToTags_Products_ProductId",
                table: "ProductToTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductToTags_Tags_TagId",
                table: "ProductToTags");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductToTags",
                table: "ProductToTags");

            migrationBuilder.RenameTable(
                name: "ProductToTags",
                newName: "ProductToTag");

            migrationBuilder.RenameIndex(
                name: "IX_ProductToTags_TagId",
                table: "ProductToTag",
                newName: "IX_ProductToTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductToTags_ProductId",
                table: "ProductToTag",
                newName: "IX_ProductToTag_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductToTag",
                table: "ProductToTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductToTag_Products_ProductId",
                table: "ProductToTag",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductToTag_Tags_TagId",
                table: "ProductToTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
