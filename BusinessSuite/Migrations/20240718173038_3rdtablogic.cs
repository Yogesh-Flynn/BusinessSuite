using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessSuite.Migrations
{
    /// <inheritdoc />
    public partial class _3rdtablogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "MarketingId",
                table: "Marketings_Products",
                newName: "MarketingsId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Marketings_Products",
                newName: "ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_Marketings_Products_MarketingId",
                table: "Marketings_Products",
                newName: "IX_Marketings_Products_MarketingsId");

            migrationBuilder.RenameColumn(
                name: "MarketingId",
                table: "Campaigns_Marketings",
                newName: "MarketingsId");

            migrationBuilder.RenameColumn(
                name: "CampaignId",
                table: "Campaigns_Marketings",
                newName: "CampaignsId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Marketings_MarketingId",
                table: "Campaigns_Marketings",
                newName: "IX_Campaigns_Marketings_MarketingsId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Campaigns_Customers",
                newName: "CustomersId");

            migrationBuilder.RenameColumn(
                name: "CampaignId",
                table: "Campaigns_Customers",
                newName: "CampaignsId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Customers_CustomerId",
                table: "Campaigns_Customers",
                newName: "IX_Campaigns_Customers_CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Customers_Campaigns_CampaignsId",
                table: "Campaigns_Customers",
                column: "CampaignsId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Customers_Customers_CustomersId",
                table: "Campaigns_Customers",
                column: "CustomersId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Marketings_Campaigns_CampaignsId",
                table: "Campaigns_Marketings",
                column: "CampaignsId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Marketings_Marketings_MarketingsId",
                table: "Campaigns_Marketings",
                column: "MarketingsId",
                principalTable: "Marketings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Products_Marketings_MarketingsId",
                table: "Marketings_Products",
                column: "MarketingsId",
                principalTable: "Marketings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketings_Products_Products_ProductsId",
                table: "Marketings_Products",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Customers_Campaigns_CampaignsId",
                table: "Campaigns_Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Customers_Customers_CustomersId",
                table: "Campaigns_Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Marketings_Campaigns_CampaignsId",
                table: "Campaigns_Marketings");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Marketings_Marketings_MarketingsId",
                table: "Campaigns_Marketings");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Products_Marketings_MarketingsId",
                table: "Marketings_Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketings_Products_Products_ProductsId",
                table: "Marketings_Products");

            migrationBuilder.RenameColumn(
                name: "MarketingsId",
                table: "Marketings_Products",
                newName: "MarketingId");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "Marketings_Products",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Marketings_Products_MarketingsId",
                table: "Marketings_Products",
                newName: "IX_Marketings_Products_MarketingId");

            migrationBuilder.RenameColumn(
                name: "MarketingsId",
                table: "Campaigns_Marketings",
                newName: "MarketingId");

            migrationBuilder.RenameColumn(
                name: "CampaignsId",
                table: "Campaigns_Marketings",
                newName: "CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Marketings_MarketingsId",
                table: "Campaigns_Marketings",
                newName: "IX_Campaigns_Marketings_MarketingId");

            migrationBuilder.RenameColumn(
                name: "CustomersId",
                table: "Campaigns_Customers",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "CampaignsId",
                table: "Campaigns_Customers",
                newName: "CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Customers_CustomersId",
                table: "Campaigns_Customers",
                newName: "IX_Campaigns_Customers_CustomerId");

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
    }
}
