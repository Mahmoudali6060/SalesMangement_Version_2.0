﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(EntitiesDbContext))]
    [Migration("20190306193421_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("TabarakV2")
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Database.Entities.Farmer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Farmers");
                });

            modelBuilder.Entity("Database.Entities.OrderDetails", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<long>("OrderHeaderId");

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity");

                    b.Property<long>("SellerId");

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.HasIndex("SellerId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Database.Entities.OrderHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<long>("FarmerId");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Number");

                    b.Property<DateTime>("OrderDate");

                    b.HasKey("Id");

                    b.HasIndex("FarmerId");

                    b.ToTable("OrderHeaders");
                });

            modelBuilder.Entity("Database.Entities.Order_Purechase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<long>("OrderHeaderId");

                    b.Property<long>("PurechasesHeaderId");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.HasIndex("PurechasesHeaderId");

                    b.ToTable("Order_Purechases");
                });

            modelBuilder.Entity("Database.Entities.PurechasesDetials", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<decimal>("Price");

                    b.Property<long>("PurechasesHeaderId");

                    b.Property<int>("Quantity");

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("PurechasesHeaderId");

                    b.ToTable("PurechasesDetials");
                });

            modelBuilder.Entity("Database.Entities.PurechasesHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Commission");

                    b.Property<DateTime>("Created");

                    b.Property<long>("FarmerId");

                    b.Property<DateTime>("Modified");

                    b.Property<decimal>("Nawlon");

                    b.Property<string>("Notes");

                    b.Property<DateTime>("PurechasesDate");

                    b.Property<decimal>("Total");

                    b.HasKey("Id");

                    b.HasIndex("FarmerId");

                    b.ToTable("PurechasesHeaders");
                });

            modelBuilder.Entity("Database.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Database.Entities.SalesinvoicesDetials", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<DateTime>("OrderDate");

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity");

                    b.Property<long>("SalesinvoicesHeaderId");

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("SalesinvoicesHeaderId");

                    b.ToTable("SalesinvoicesDetials");
                });

            modelBuilder.Entity("Database.Entities.SalesinvoicesHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Notes");

                    b.Property<DateTime>("SalesinvoicesDate");

                    b.Property<long>("SellerId");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("SalesinvoicesHeaders");
                });

            modelBuilder.Entity("Database.Entities.Seller", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("Database.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Password");

                    b.Property<long>("RoleId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Database.Entities.OrderDetails", b =>
                {
                    b.HasOne("Database.Entities.OrderHeader", "OrderHeader")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Database.Entities.Seller", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.OrderHeader", b =>
                {
                    b.HasOne("Database.Entities.Farmer", "Farmer")
                        .WithMany("OrderHeader")
                        .HasForeignKey("FarmerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.Order_Purechase", b =>
                {
                    b.HasOne("Database.Entities.OrderHeader", "OrderHeader")
                        .WithMany()
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Database.Entities.PurechasesHeader", "PurechasesHeaders")
                        .WithMany()
                        .HasForeignKey("PurechasesHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.PurechasesDetials", b =>
                {
                    b.HasOne("Database.Entities.PurechasesHeader", "PurechasesHeader")
                        .WithMany("PurechasesDetialsList")
                        .HasForeignKey("PurechasesHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.PurechasesHeader", b =>
                {
                    b.HasOne("Database.Entities.Farmer", "Farmer")
                        .WithMany("PurechasesHeader")
                        .HasForeignKey("FarmerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.SalesinvoicesDetials", b =>
                {
                    b.HasOne("Database.Entities.SalesinvoicesHeader", "PurechasesHeader")
                        .WithMany("SalesinvoicesDetialsList")
                        .HasForeignKey("SalesinvoicesHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.SalesinvoicesHeader", b =>
                {
                    b.HasOne("Database.Entities.Seller", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Database.Entities.User", b =>
                {
                    b.HasOne("Database.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
