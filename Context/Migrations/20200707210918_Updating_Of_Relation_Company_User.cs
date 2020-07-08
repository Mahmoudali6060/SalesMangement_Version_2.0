using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Updating_Of_Relation_Company_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companys_Users_UserId",
                schema: "SalesManagement",
                table: "Companys");

            migrationBuilder.DropIndex(
                name: "IX_Companys_UserId",
                schema: "SalesManagement",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "SalesManagement",
                table: "Companys");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                schema: "SalesManagement",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companys_CompanyId",
                schema: "SalesManagement",
                table: "Users",
                column: "CompanyId",
                principalSchema: "SalesManagement",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companys_CompanyId",
                schema: "SalesManagement",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                schema: "SalesManagement",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "SalesManagement",
                table: "Companys",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companys_UserId",
                schema: "SalesManagement",
                table: "Companys",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_Users_UserId",
                schema: "SalesManagement",
                table: "Companys",
                column: "UserId",
                principalSchema: "SalesManagement",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
