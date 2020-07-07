using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SalesManagement");

            migrationBuilder.CreateTable(
                name: "Farmers",
                schema: "SalesManagement",
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
                schema: "SalesManagement",
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
                name: "Safes",
                schema: "SalesManagement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    AccountTypeId = table.Column<int>(nullable: false),
                    AccountId = table.Column<long>(nullable: false),
                    OtherAccountName = table.Column<string>(nullable: true),
                    Incoming = table.Column<decimal>(nullable: false),
                    Outcoming = table.Column<decimal>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    IsHidden = table.Column<bool>(nullable: false),
                    HeaderId = table.Column<long>(nullable: false),
                    OrderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Safes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                schema: "SalesManagement",
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
                schema: "SalesManagement",
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
                        principalSchema: "SalesManagement",
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurechasesHeaders",
                schema: "SalesManagement",
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
                    CommissionRate = table.Column<decimal>(nullable: false),
                    Nawlon = table.Column<decimal>(nullable: false),
                    Gift = table.Column<decimal>(nullable: false),
                    Descent = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurechasesHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurechasesHeaders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalSchema: "SalesManagement",
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "SalesManagement",
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
                    RoleId = table.Column<long>(nullable: false),
                    CompanyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "SalesManagement",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesinvoicesHeaders",
                schema: "SalesManagement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    SalesinvoicesDate = table.Column<DateTime>(nullable: false),
                    SellerId = table.Column<long>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    ByaaTotal = table.Column<decimal>(nullable: false),
                    MashalTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesinvoicesHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesinvoicesHeaders_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "SalesManagement",
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "SalesManagement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    SellingPrice = table.Column<decimal>(nullable: false),
                    SellerId = table.Column<long>(nullable: false),
                    OrderHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalSchema: "SalesManagement",
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "SalesManagement",
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Purechases",
                schema: "SalesManagement",
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
                        principalSchema: "SalesManagement",
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                   
                });

            migrationBuilder.CreateTable(
                name: "PurechasesDetials",
                schema: "SalesManagement",
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
                        principalSchema: "SalesManagement",
                        principalTable: "PurechasesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companys",
                schema: "SalesManagement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    LogoUrl = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companys_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "SalesManagement",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesinvoicesDetials",
                schema: "SalesManagement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Mashal = table.Column<decimal>(nullable: false),
                    Byaa = table.Column<decimal>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    SalesinvoicesHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesinvoicesDetials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesinvoicesDetials_SalesinvoicesHeaders_SalesinvoicesHeaderId",
                        column: x => x.SalesinvoicesHeaderId,
                        principalSchema: "SalesManagement",
                        principalTable: "SalesinvoicesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companys_UserId",
                schema: "SalesManagement",
                table: "Companys",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Purechases_OrderHeaderId",
                schema: "SalesManagement",
                table: "Order_Purechases",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Purechases_PurechasesHeaderId",
                schema: "SalesManagement",
                table: "Order_Purechases",
                column: "PurechasesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                schema: "SalesManagement",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_SellerId",
                schema: "SalesManagement",
                table: "OrderDetails",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_FarmerId",
                schema: "SalesManagement",
                table: "OrderHeaders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurechasesDetials_PurechasesHeaderId",
                schema: "SalesManagement",
                table: "PurechasesDetials",
                column: "PurechasesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurechasesHeaders_FarmerId",
                schema: "SalesManagement",
                table: "PurechasesHeaders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesinvoicesDetials_SalesinvoicesHeaderId",
                schema: "SalesManagement",
                table: "SalesinvoicesDetials",
                column: "SalesinvoicesHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesinvoicesHeaders_SellerId",
                schema: "SalesManagement",
                table: "SalesinvoicesHeaders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "SalesManagement",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companys",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "Order_Purechases",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "PurechasesDetials",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "Safes",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "SalesinvoicesDetials",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "OrderHeaders",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "PurechasesHeaders",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "SalesinvoicesHeaders",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "Farmers",
                schema: "SalesManagement");

            migrationBuilder.DropTable(
                name: "Sellers",
                schema: "SalesManagement");
        }
    }
}
