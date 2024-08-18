using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations.CRMDb
{
    /// <inheritdoc />
    public partial class x1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Campaigns_Marketings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Campaigns_Marketings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
