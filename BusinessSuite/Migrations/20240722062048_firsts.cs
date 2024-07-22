using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations
{
    /// <inheritdoc />
    public partial class firsts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Marketings_Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Campaigns_Marketings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Marketings_Customers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Campaigns_Marketings");
        }
    }
}
