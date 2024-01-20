﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Base;

#nullable disable

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Migrations
{
    [DbContext(typeof(DbSession))]
    partial class DbSessionModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Checked")
                        .HasColumnType("bit");

                    b.Property<string>("CreatorAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("OperatorAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SavedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("AddedTime")
                        .HasDatabaseName("IX_AddedTime");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AddedTime"));

                    b.HasIndex("Number")
                        .IsUnique()
                        .HasDatabaseName("IX_Number")
                        .HasFilter("[Number] IS NOT NULL");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Age")
                        .HasPrecision(10, 3)
                        .HasColumnType("decimal(10,3)");

                    b.Property<string>("CreatorAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("OperatorAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrivateKey")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("SavedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("AddedTime")
                        .HasDatabaseName("IX_AddedTime");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AddedTime"));

                    b.HasIndex("Number")
                        .IsUnique()
                        .HasDatabaseName("IX_Number")
                        .HasFilter("[Number] IS NOT NULL");

                    b.HasIndex("PrivateKey")
                        .IsUnique()
                        .HasDatabaseName("IX_PrivateKey")
                        .HasFilter("[PrivateKey] IS NOT NULL");

                    b.ToTable("User", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
