using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaigns_Customers");

            migrationBuilder.DropTable(
                name: "Marketings_Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Marketings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Marketings_Customers",
                columns: table => new
                {
                    MarketingId = table.Column<int>(type: "int", nullable: false),
                    CustomersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketings_Customers", x => new { x.MarketingId, x.CustomersId });
                    table.ForeignKey(
                        name: "FK_Marketings_Customers_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marketings_Customers_Marketings_MarketingId",
                        column: x => x.MarketingId,
                        principalTable: "Marketings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Marketings_ProductId",
                table: "Marketings",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marketings_Customers_CustomersId",
                table: "Marketings_Customers",
                column: "CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Products_ProductId",
                table: "Marketings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Products_ProductId",
                table: "Marketings");

            migrationBuilder.DropTable(
                name: "Marketings_Customers");

            migrationBuilder.DropIndex(
                name: "IX_Marketings_ProductId",
                table: "Marketings");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Marketings");

            migrationBuilder.CreateTable(
                name: "Campaigns_Customers",
                columns: table => new
                {
                    CampaignsId = table.Column<int>(type: "int", nullable: false),
                    CustomersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns_Customers", x => new { x.CampaignsId, x.CustomersId });
                    table.ForeignKey(
                        name: "FK_Campaigns_Customers_Campaigns_CampaignsId",
                        column: x => x.CampaignsId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaigns_Customers_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marketings_Products",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    MarketingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketings_Products", x => new { x.ProductsId, x.MarketingsId });
                    table.ForeignKey(
                        name: "FK_Marketings_Products_Marketings_MarketingsId",
                        column: x => x.MarketingsId,
                        principalTable: "Marketings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marketings_Products_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_Customers_CustomersId",
                table: "Campaigns_Customers",
                column: "CustomersId");

            migrationBuilder.CreateIndex(
                name: "IX_Marketings_Products_MarketingsId",
                table: "Marketings_Products",
                column: "MarketingsId");
        }
    }
}
