using Microsoft.EntityFrameworkCore.Migrations;

namespace TMOffersClients.Migrations
{
    public partial class PKMeters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OffersPorducts",
                table: "OffersPorducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OffersPorducts",
                table: "OffersPorducts",
                columns: new[] { "OfferID", "ProductID", "Meters" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OffersPorducts",
                table: "OffersPorducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OffersPorducts",
                table: "OffersPorducts",
                columns: new[] { "OfferID", "ProductID" });
        }
    }
}
