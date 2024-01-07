﻿// <auto-generated />
using System;
using EIRS.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EIRS.Admin.Migrations
{
    [DbContext(typeof(EirsDbContext))]
    partial class EirsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EIRS.Admin.Models.AssessmentAndItemRollOver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<decimal>("AssessmentAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("AssessmentItemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssessmentRuleCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssessmentRuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TaxAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TaxBaseAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TaxMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxYear")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AssessmentAndItemRollOver");
                });

            modelBuilder.Entity("EIRS.Admin.Models.AssessmentRuleRollover", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ARAIID")
                        .HasColumnType("int");

                    b.Property<string>("Active")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("AssessmentAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AssessmentItemID")
                        .HasColumnType("int");

                    b.Property<string>("AssessmentRuleCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AssessmentRuleID")
                        .HasColumnType("int");

                    b.Property<string>("AssessmentRuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Createdby")
                        .HasColumnType("int");

                    b.Property<DateTime>("Createddate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Paymentfrequencyid")
                        .HasColumnType("int");

                    b.Property<int>("Profileid")
                        .HasColumnType("int");

                    b.Property<int>("RuleRunId")
                        .HasColumnType("int");

                    b.Property<int>("Taxyear")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AssessmentRuleRollover");
                });
#pragma warning restore 612, 618
        }
    }
}
