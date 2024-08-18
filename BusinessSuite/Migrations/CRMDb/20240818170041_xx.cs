using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations.CRMDb
{
    /// <inheritdoc />
    public partial class xx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageData",
                table: "Marketings",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Marketings",
                newName: "ImageData");
        }
    }
}
