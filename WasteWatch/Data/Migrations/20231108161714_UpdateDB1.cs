using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteWatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageProcessedId",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageProcessedId",
                table: "Images",
                type: "int",
                nullable: true);
        }
    }
}
