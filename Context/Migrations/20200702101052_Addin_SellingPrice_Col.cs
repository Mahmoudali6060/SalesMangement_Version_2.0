using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Addin_SellingPrice_Col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                schema: "TabarakV2",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPrice",
                schema: "TabarakV2",
                table: "OrderDetails");
        }
    }
}
