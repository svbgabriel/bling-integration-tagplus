﻿// <auto-generated />
using BlingIntegrationTagplus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlingIntegrationTagplus.Migrations
{
    [DbContext(typeof(IntegrationContext))]
    [Migration("20200611031502_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("BlingIntegrationTagplus.SettingString", b =>
                {
                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<string>("value")
                        .HasColumnType("TEXT");

                    b.HasKey("name");

                    b.ToTable("SettingStrings");
                });
#pragma warning restore 612, 618
        }
    }
}