using Microsoft.EntityFrameworkCore.Migrations;

namespace TMOffersClients.Migrations
{
    public partial class AddingMetersToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Meters",
                table: "OffersPorducts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meters",
                table: "OffersPorducts");
        }
    }
}
