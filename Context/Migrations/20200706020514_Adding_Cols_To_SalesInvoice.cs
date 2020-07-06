using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Adding_Cols_To_SalesInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ByaaTotal",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MashalTotal",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Byaa",
                schema: "TabarakV2",
                table: "SalesinvoicesDetials",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Mashal",
                schema: "TabarakV2",
                table: "SalesinvoicesDetials",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ByaaTotal",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders");

            migrationBuilder.DropColumn(
                name: "MashalTotal",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders");

            migrationBuilder.DropColumn(
                name: "Total",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders");

            migrationBuilder.DropColumn(
                name: "Byaa",
                schema: "TabarakV2",
                table: "SalesinvoicesDetials");

            migrationBuilder.DropColumn(
                name: "Mashal",
                schema: "TabarakV2",
                table: "SalesinvoicesDetials");
        }
    }
}
