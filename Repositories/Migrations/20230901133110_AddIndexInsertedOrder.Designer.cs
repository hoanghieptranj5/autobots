﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repositories.Models;

#nullable disable

namespace Repositories.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230901133110_AddIndexInsertedOrder")]
    partial class AddIndexInsertedOrder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Repositories.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CollectedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CollectedFrom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("Repositories.Models.ElectricCalculator.ElectricPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("From")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<float>("StandardPrice")
                        .HasColumnType("real");

                    b.Property<int>("To")
                        .HasColumnType("int");

                    b.Property<float>("Usage")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("ElectricPrices");
                });

            modelBuilder.Entity("Repositories.Models.HanziCollector.Hanzi", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Cantonese")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HanViet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InsertedOrder")
                        .HasColumnType("int");

                    b.Property<string>("MeaningInVietnamese")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pinyin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Stroke")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InsertedOrder");

                    b.ToTable("Hanzis");
                });
#pragma warning restore 612, 618
        }
    }
}
