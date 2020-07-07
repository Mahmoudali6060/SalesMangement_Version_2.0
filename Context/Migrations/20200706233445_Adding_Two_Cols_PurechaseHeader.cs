using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Adding_Two_Cols_PurechaseHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Descent",
                schema: "TabarakV2",
                table: "PurechasesHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gift",
                schema: "TabarakV2",
                table: "PurechasesHeaders",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descent",
                schema: "TabarakV2",
                table: "PurechasesHeaders");

            migrationBuilder.DropColumn(
                name: "Gift",
                schema: "TabarakV2",
                table: "PurechasesHeaders");
        }
    }
}
