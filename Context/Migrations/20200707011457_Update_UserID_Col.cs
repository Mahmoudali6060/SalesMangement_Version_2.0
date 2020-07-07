using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Update_UserID_Col : Migration
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

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "SalesManagement",
                table: "Users",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "SalesManagement",
                table: "Companys",
                nullable: true,
                oldClrType: typeof(long));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companys_Users_UserId",
                schema: "SalesManagement",
                table: "Companys");

            migrationBuilder.DropIndex(
                name: "IX_Companys_UserId",
                schema: "SalesManagement",
                table: "Companys");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "SalesManagement",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "SalesManagement",
                table: "Companys",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companys_UserId",
                schema: "SalesManagement",
                table: "Companys",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_Users_UserId",
                schema: "SalesManagement",
                table: "Companys",
                column: "UserId",
                principalSchema: "SalesManagement",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
