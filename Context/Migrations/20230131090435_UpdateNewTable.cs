using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class UpdateNewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "SalesManagement",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "SalesManagement",
                table: "Sellers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrinted",
                schema: "SalesManagement",
                table: "SalesinvoicesHeaders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FarmerId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransfered",
                schema: "SalesManagement",
                table: "Safes",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Expense",
                schema: "SalesManagement",
                table: "PurechasesHeaders",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrinted",
                schema: "SalesManagement",
                table: "PurechasesHeaders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransfered",
                schema: "SalesManagement",
                table: "PurechasesHeaders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransfered",
                schema: "SalesManagement",
                table: "OrderHeaders",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "SalesManagement",
                table: "Farmers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_SalesinvoicesDetials_FarmerId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesinvoicesDetials_Farmers_FarmerId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials",
                column: "FarmerId",
                principalSchema: "SalesManagement",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesinvoicesDetials_Farmers_FarmerId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials");

            migrationBuilder.DropIndex(
                name: "IX_SalesinvoicesDetials_FarmerId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "SalesManagement",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "SalesManagement",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "IsPrinted",
                schema: "SalesManagement",
                table: "SalesinvoicesHeaders");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials");

            migrationBuilder.DropColumn(
                name: "IsTransfered",
                schema: "SalesManagement",
                table: "Safes");

            migrationBuilder.DropColumn(
                name: "IsPrinted",
                schema: "SalesManagement",
                table: "PurechasesHeaders");

            migrationBuilder.DropColumn(
                name: "IsTransfered",
                schema: "SalesManagement",
                table: "PurechasesHeaders");

            migrationBuilder.DropColumn(
                name: "IsTransfered",
                schema: "SalesManagement",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "SalesManagement",
                table: "Farmers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Expense",
                schema: "SalesManagement",
                table: "PurechasesHeaders",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
