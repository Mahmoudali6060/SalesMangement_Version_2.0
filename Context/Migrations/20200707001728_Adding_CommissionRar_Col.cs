using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Adding_CommissionRar_Col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                schema: "TabarakV2",
                table: "PurechasesHeaders",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionRate",
                schema: "TabarakV2",
                table: "PurechasesHeaders");
        }
    }
}
