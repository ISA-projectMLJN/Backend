﻿// <auto-generated />
using System;
using Medicina.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Medicina.Migrations.EquipmentTracking
{
<<<<<<<< HEAD:Medicina/Migrations/CompanyRate/20240131201818_rates.Designer.cs
    [DbContext(typeof(CompanyRateContext))]
    [Migration("20240131201818_rates")]
    partial class rates
========
    [DbContext(typeof(EquipmentTrackingContext))]
    [Migration("20240131015436_trac")]
    partial class trac
>>>>>>>> development:Medicina/Migrations/EquipmentTracking/20240131015436_trac.Designer.cs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Medicina.Models.EquipmentTracking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("LatitudeB")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

<<<<<<<< HEAD:Medicina/Migrations/CompanyRate/20240131201818_rates.Designer.cs
                    b.Property<bool>("LowQuality")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("WideSelection")
                        .HasColumnType("bit");
========
                    b.Property<double>("LongitudeB")
                        .HasColumnType("float");
>>>>>>>> development:Medicina/Migrations/EquipmentTracking/20240131015436_trac.Designer.cs

                    b.HasKey("Id");

                    b.ToTable("EquipmentTracking");
                });
#pragma warning restore 612, 618
        }
    }
}
