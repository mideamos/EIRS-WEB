﻿// <auto-generated />
using System;
using EIRS.Web.GISModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EIRS.Web.Migrations
{
    [DbContext(typeof(EIRSContext))]
    [Migration("20220624192936_new")]
    partial class @new
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileAssessment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AssessmentID")
                        .HasColumnType("bigint");

                    b.Property<string>("AssessmentYear")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<long>("FileId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileAssessment");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileAssessmentItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssessmentAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("AssessmentID")
                        .HasColumnType("bigint");

                    b.Property<string>("AssetNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileAssessmentItem");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileAsset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnyOccupants")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetAge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetBillingZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetCofO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetCompletionYear")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetDistrict")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetFootprintPresent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetFurnished")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetLGA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetLatitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetLongitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetModifiedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetOffStreet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetPerimeter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetPurpose")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetRoadName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetSubType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetSubzone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetUse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssetWard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<string>("ElectricConnection")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FileId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HasBuildings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HasFence")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HasGenerator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HasSwimmingPool")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoldingType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfCompletion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumberOfBldgs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumberOfFloors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupancyType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupierStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoofMaterial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SewageAccess")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolidWasteCollectionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOfRepair")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportingDocument")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleDocument")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WallMaterial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WaterConnectionType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileAsset");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileHolder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreationDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<long>("FileId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileHolder");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileInvoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<long>("FileId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("isReversal")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileInvoice");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileInvoiceItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("InvoiceID")
                        .HasColumnType("bigint");

                    b.Property<string>("RevenueHeadId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileInvoiceItem");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.GISFileParty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AcquisitionDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSaved")
                        .HasColumnType("datetime2");

                    b.Property<long>("FileId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyDOB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyExtID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyFullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyGender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyMaritalStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyMiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyNIN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyNationality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyOccupation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyPhone1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyPhone2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyRelation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyTIN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartyType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GISFileParty");
                });

            modelBuilder.Entity("EIRS.Web.GISModels.Gistesting", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.ToTable("GISTesting");
                });
#pragma warning restore 612, 618
        }
    }
}
