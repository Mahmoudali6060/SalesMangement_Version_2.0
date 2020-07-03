using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class updateing_Safe_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherAccountName",
                schema: "TabarakV2",
                table: "Safes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherAccountName",
                schema: "TabarakV2",
                table: "Safes");
        }
    }
}
