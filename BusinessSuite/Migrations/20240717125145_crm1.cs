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
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Customers_Campaigns_CampaignId",
                table: "Campaign_Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Customers_Customers_CustomerId",
                table: "Campaign_Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Marketing_Campaigns_CampaignId",
                table: "Campaigns_Marketing");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Marketing_Marketings_MarketingId",
                table: "Campaigns_Marketing");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Product_Marketings_MarketingId",
                table: "Marketings_Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Product_Products_ProductId",
                table: "Marketings_Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marketings_Product",
                table: "Marketings_Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns_Marketing",
                table: "Campaigns_Marketing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaign_Customers",
                table: "Campaign_Customers");

            migrationBuilder.RenameTable(
                name: "Marketings_Product",
                newName: "Marketings_Products");

            migrationBuilder.RenameTable(
                name: "Campaigns_Marketing",
                newName: "Campaigns_Marketings");

            migrationBuilder.RenameTable(
                name: "Campaign_Customers",
                newName: "Campaigns_Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Marketings_Product_MarketingId",
                table: "Marketings_Products",
                newName: "IX_Marketings_Products_MarketingId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Marketing_MarketingId",
                table: "Campaigns_Marketings",
                newName: "IX_Campaigns_Marketings_MarketingId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_Customers_CustomerId",
                table: "Campaigns_Customers",
                newName: "IX_Campaigns_Customers_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marketings_Products",
                table: "Marketings_Products",
                columns: new[] { "ProductId", "MarketingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns_Marketings",
                table: "Campaigns_Marketings",
                columns: new[] { "CampaignId", "MarketingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns_Customers",
                table: "Campaigns_Customers",
                columns: new[] { "CampaignId", "CustomerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Customers_Campaigns_CampaignId",
                table: "Campaigns_Customers",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Customers_Customers_CustomerId",
                table: "Campaigns_Customers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Marketings_Campaigns_CampaignId",
                table: "Campaigns_Marketings",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Marketings_Marketings_MarketingId",
                table: "Campaigns_Marketings",
                column: "MarketingId",
                principalTable: "Marketings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Products_Marketings_MarketingId",
                table: "Marketings_Products",
                column: "MarketingId",
                principalTable: "Marketings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Products_Products_ProductId",
                table: "Marketings_Products",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Customers_Campaigns_CampaignId",
                table: "Campaigns_Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Customers_Customers_CustomerId",
                table: "Campaigns_Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Marketings_Campaigns_CampaignId",
                table: "Campaigns_Marketings");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Marketings_Marketings_MarketingId",
                table: "Campaigns_Marketings");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Products_Marketings_MarketingId",
                table: "Marketings_Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Products_Products_ProductId",
                table: "Marketings_Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marketings_Products",
                table: "Marketings_Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns_Marketings",
                table: "Campaigns_Marketings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns_Customers",
                table: "Campaigns_Customers");

            migrationBuilder.RenameTable(
                name: "Marketings_Products",
                newName: "Marketings_Product");

            migrationBuilder.RenameTable(
                name: "Campaigns_Marketings",
                newName: "Campaigns_Marketing");

            migrationBuilder.RenameTable(
                name: "Campaigns_Customers",
                newName: "Campaign_Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Marketings_Products_MarketingId",
                table: "Marketings_Product",
                newName: "IX_Marketings_Product_MarketingId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Marketings_MarketingId",
                table: "Campaigns_Marketing",
                newName: "IX_Campaigns_Marketing_MarketingId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Customers_CustomerId",
                table: "Campaign_Customers",
                newName: "IX_Campaign_Customers_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marketings_Product",
                table: "Marketings_Product",
                columns: new[] { "ProductId", "MarketingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns_Marketing",
                table: "Campaigns_Marketing",
                columns: new[] { "CampaignId", "MarketingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaign_Customers",
                table: "Campaign_Customers",
                columns: new[] { "CampaignId", "CustomerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Customers_Campaigns_CampaignId",
                table: "Campaign_Customers",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Customers_Customers_CustomerId",
                table: "Campaign_Customers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Marketing_Campaigns_CampaignId",
                table: "Campaigns_Marketing",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Marketing_Marketings_MarketingId",
                table: "Campaigns_Marketing",
                column: "MarketingId",
                principalTable: "Marketings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Product_Marketings_MarketingId",
                table: "Marketings_Product",
                column: "MarketingId",
                principalTable: "Marketings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Product_Products_ProductId",
                table: "Marketings_Product",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
