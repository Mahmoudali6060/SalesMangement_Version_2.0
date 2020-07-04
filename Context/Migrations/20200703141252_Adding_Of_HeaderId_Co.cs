using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Adding_Of_HeaderId_Co : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HeaderId",
                schema: "TabarakV2",
                table: "Safes",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderId",
                schema: "TabarakV2",
                table: "Safes");
        }
    }
}
