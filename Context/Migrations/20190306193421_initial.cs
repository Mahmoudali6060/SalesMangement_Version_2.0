using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TabarakV2");

            migrationBuilder.CreateTable(
                name: "Farmers",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    FarmerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalSchema: "TabarakV2",
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurechasesHeaders",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    PurechasesDate = table.Column<DateTime>(nullable: false),
                    FarmerId = table.Column<long>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    Commission = table.Column<decimal>(nullable: false),
                    Nawlon = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurechasesHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurechasesHeaders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalSchema: "TabarakV2",
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "TabarakV2",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesinvoicesHeaders",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    SalesinvoicesDate = table.Column<DateTime>(nullable: false),
                    SellerId = table.Column<long>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesinvoicesHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesinvoicesHeaders_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "TabarakV2",
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    SellerId = table.Column<long>(nullable: false),
                    OrderHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalSchema: "TabarakV2",
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "TabarakV2",
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Purechases",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    PurechasesHeaderId = table.Column<long>(nullable: false),
                    OrderHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Purechases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Purechases_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalSchema: "TabarakV2",
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Purechases_PurechasesHeaders_PurechasesHeaderId",
                        column: x => x.PurechasesHeaderId,
                        principalSchema: "TabarakV2",
                        principalTable: "PurechasesHeaders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurechasesDetials",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PurechasesHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurechasesDetials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurechasesDetials_PurechasesHeaders_PurechasesHeaderId",
                        column: x => x.PurechasesHeaderId,
                        principalSchema: "TabarakV2",
                        principalTable: "PurechasesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesinvoicesDetials",
                schema: "TabarakV2",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    SalesinvoicesHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesinvoicesDetials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesinvoicesDetials_SalesinvoicesHeaders_SalesinvoicesHeaderId",
                        column: x => x.SalesinvoicesHeaderId,
                        principalSchema: "TabarakV2",
                        principalTable: "SalesinvoicesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Purechases_OrderHeaderId",
                schema: "TabarakV2",
                table: "Order_Purechases",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Purechases_PurechasesHeaderId",
                schema: "TabarakV2",
                table: "Order_Purechases",
                column: "PurechasesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                schema: "TabarakV2",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_SellerId",
                schema: "TabarakV2",
                table: "OrderDetails",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_FarmerId",
                schema: "TabarakV2",
                table: "OrderHeaders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurechasesDetials_PurechasesHeaderId",
                schema: "TabarakV2",
                table: "PurechasesDetials",
                column: "PurechasesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurechasesHeaders_FarmerId",
                schema: "TabarakV2",
                table: "PurechasesHeaders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesinvoicesDetials_SalesinvoicesHeaderId",
                schema: "TabarakV2",
                table: "SalesinvoicesDetials",
                column: "SalesinvoicesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesinvoicesHeaders_SellerId",
                schema: "TabarakV2",
                table: "SalesinvoicesHeaders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "TabarakV2",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_Purechases",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "PurechasesDetials",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "SalesinvoicesDetials",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "OrderHeaders",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "PurechasesHeaders",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "SalesinvoicesHeaders",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "Farmers",
                schema: "TabarakV2");

            migrationBuilder.DropTable(
                name: "Sellers",
                schema: "TabarakV2");
        }
    }
}
