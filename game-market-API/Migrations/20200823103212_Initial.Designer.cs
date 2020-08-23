﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using game_market_API.Models;

namespace game_market_API.Migrations
{
    [DbContext(typeof(GameMarketDbContext))]
    [Migration("20200823103212_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0-preview.7.20365.15");

            modelBuilder.Entity("game_market_API.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EMail")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("game_market_API.Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("game_market_API.Models.GameKey", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaymentSessionID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VendorID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("GameID");

                    b.HasIndex("PaymentSessionID");

                    b.HasIndex("VendorID");

                    b.ToTable("GameKeys");
                });

            modelBuilder.Entity("game_market_API.Models.PaymentSession", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("PaymentSessions");
                });

            modelBuilder.Entity("game_market_API.Models.Vendor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("URL")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("game_market_API.Models.GameKey", b =>
                {
                    b.HasOne("game_market_API.Models.Customer", "Customer")
                        .WithMany("GameKeys")
                        .HasForeignKey("CustomerID");

                    b.HasOne("game_market_API.Models.Game", "Game")
                        .WithMany("GameKeys")
                        .HasForeignKey("GameID");

                    b.HasOne("game_market_API.Models.PaymentSession", null)
                        .WithMany("GameKeys")
                        .HasForeignKey("PaymentSessionID");

                    b.HasOne("game_market_API.Models.Vendor", "Vendor")
                        .WithMany("GameKeys")
                        .HasForeignKey("VendorID");
                });
#pragma warning restore 612, 618
        }
    }
}
