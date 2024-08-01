using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations
{
    /// <inheritdoc />
    public partial class xxxxxxxxxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Websites_Companies_CompanyId1",
                table: "Websites");

            migrationBuilder.DropIndex(
                name: "IX_Websites_CompanyId1",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Websites");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId1",
                table: "Websites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Websites_CompanyId1",
                table: "Websites",
                column: "CompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Websites_Companies_CompanyId1",
                table: "Websites",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
