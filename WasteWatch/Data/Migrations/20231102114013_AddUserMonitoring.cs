using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteWatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessedById",
                table: "ImagesProcessed",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesProcessed_ProcessedById",
                table: "ImagesProcessed",
                column: "ProcessedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesProcessed_AspNetUsers_ProcessedById",
                table: "ImagesProcessed",
                column: "ProcessedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagesProcessed_AspNetUsers_ProcessedById",
                table: "ImagesProcessed");

            migrationBuilder.DropIndex(
                name: "IX_ImagesProcessed_ProcessedById",
                table: "ImagesProcessed");

            migrationBuilder.DropColumn(
                name: "ProcessedById",
                table: "ImagesProcessed");
        }
    }
}
