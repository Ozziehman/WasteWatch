using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteWatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagesProcessed");

            migrationBuilder.AddColumn<string>(
                name: "Boxes",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BoxesYOLO",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProcessedById",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProcessedById",
                table: "Images",
                column: "ProcessedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_ProcessedById",
                table: "Images",
                column: "ProcessedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_ProcessedById",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProcessedById",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Boxes",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "BoxesYOLO",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProcessedById",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "ImagesProcessed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Boxes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoxesYOLO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesProcessed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesProcessed_AspNetUsers_ProcessedById",
                        column: x => x.ProcessedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagesProcessed_ProcessedById",
                table: "ImagesProcessed",
                column: "ProcessedById");
        }
    }
}
