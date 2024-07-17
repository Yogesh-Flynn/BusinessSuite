using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations
{
    /// <inheritdoc />
    public partial class crm1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignCustomers");

            migrationBuilder.DropTable(
                name: "MarketingCampaigns");

            migrationBuilder.DropTable(
                name: "Product_Marketings");

            migrationBuilder.CreateTable(
                name: "Campaign_Customers",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign_Customers", x => new { x.CampaignId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_Campaign_Customers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaign_Customers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns_Marketing",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    MarketingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns_Marketing", x => new { x.CampaignId, x.MarketingId });
                    table.ForeignKey(
                        name: "FK_Campaigns_Marketing_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaigns_Marketing_Marketings_MarketingId",
                        column: x => x.MarketingId,
                        principalTable: "Marketings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marketings_Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MarketingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketings_Product", x => new { x.ProductId, x.MarketingId });
                    table.ForeignKey(
                        name: "FK_Marketings_Product_Marketings_MarketingId",
                        column: x => x.MarketingId,
                        principalTable: "Marketings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marketings_Product_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_Customers_CustomerId",
                table: "Campaign_Customers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_Marketing_MarketingId",
                table: "Campaigns_Marketing",
                column: "MarketingId");

            migrationBuilder.CreateIndex(
                name: "IX_Marketings_Product_MarketingId",
                table: "Marketings_Product",
                column: "MarketingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaign_Customers");

            migrationBuilder.DropTable(
                name: "Campaigns_Marketing");

            migrationBuilder.DropTable(
                name: "Marketings_Product");

            migrationBuilder.CreateTable(
                name: "CampaignCustomers",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCustomers", x => new { x.CampaignId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_CampaignCustomers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarketingCampaigns",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    MarketingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingCampaigns", x => new { x.CampaignId, x.MarketingId });
                    table.ForeignKey(
                        name: "FK_MarketingCampaigns_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarketingCampaigns_Marketings_MarketingId",
                        column: x => x.MarketingId,
                        principalTable: "Marketings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Marketings",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MarketingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Marketings", x => new { x.ProductId, x.MarketingId });
                    table.ForeignKey(
                        name: "FK_Product_Marketings_Marketings_MarketingId",
                        column: x => x.MarketingId,
                        principalTable: "Marketings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Marketings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignCustomers_CustomerId",
                table: "CampaignCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketingCampaigns_MarketingId",
                table: "MarketingCampaigns",
                column: "MarketingId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Marketings_MarketingId",
                table: "Product_Marketings",
                column: "MarketingId");
        }
    }
}
