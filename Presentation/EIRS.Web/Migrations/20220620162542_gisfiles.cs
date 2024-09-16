using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EIRS.Web.Migrations
{
    public partial class gisfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GISFileAssessment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentID = table.Column<long>(nullable: false),
                    AssessmentYear = table.Column<string>(nullable: true),
                    DateSaved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileAssessment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GISFileAssessmentItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentID = table.Column<long>(nullable: false),
                    AssetNumber = table.Column<string>(nullable: true),
                    AssessmentAmount = table.Column<string>(nullable: true),
                    DateSaved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileAssessmentItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GISFileAsset",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetNumber = table.Column<string>(nullable: true),
                    AssetName = table.Column<string>(nullable: true),
                    AssetType = table.Column<string>(nullable: true),
                    AssetSubType = table.Column<string>(nullable: true),
                    AssetLGA = table.Column<string>(nullable: true),
                    AssetDistrict = table.Column<string>(nullable: true),
                    AssetWard = table.Column<string>(nullable: true),
                    AssetBillingZone = table.Column<string>(nullable: true),
                    AssetSubzone = table.Column<string>(nullable: true),
                    AssetUse = table.Column<string>(nullable: true),
                    AssetPurpose = table.Column<string>(nullable: true),
                    AssetAddress = table.Column<string>(nullable: true),
                    AssetRoadName = table.Column<string>(nullable: true),
                    AssetOffStreet = table.Column<string>(nullable: true),
                    HoldingType = table.Column<string>(nullable: true),
                    AssetCofO = table.Column<string>(nullable: true),
                    TitleDocument = table.Column<string>(nullable: true),
                    SupportingDocument = table.Column<string>(nullable: true),
                    PartyID = table.Column<string>(nullable: true),
                    OccupierStatus = table.Column<string>(nullable: true),
                    AnyOccupants = table.Column<string>(nullable: true),
                    OccupancyType = table.Column<string>(nullable: true),
                    AssetModifiedDate = table.Column<string>(nullable: true),
                    AssetFootprintPresent = table.Column<string>(nullable: true),
                    AssetAge = table.Column<string>(nullable: true),
                    AssetCompletionYear = table.Column<string>(nullable: true),
                    AssetFurnished = table.Column<string>(nullable: true),
                    AssetSize = table.Column<string>(nullable: true),
                    AssetPerimeter = table.Column<string>(nullable: true),
                    NumberOfFloors = table.Column<string>(nullable: true),
                    AssetLatitude = table.Column<string>(nullable: true),
                    AssetLongitude = table.Column<string>(nullable: true),
                    StateOfRepair = table.Column<string>(nullable: true),
                    LevelOfCompletion = table.Column<string>(nullable: true),
                    HasGenerator = table.Column<string>(nullable: true),
                    HasSwimmingPool = table.Column<string>(nullable: true),
                    HasFence = table.Column<string>(nullable: true),
                    HasBuildings = table.Column<string>(nullable: true),
                    NumberOfBldgs = table.Column<string>(nullable: true),
                    WallMaterial = table.Column<string>(nullable: true),
                    RoofMaterial = table.Column<string>(nullable: true),
                    SewageAccess = table.Column<string>(nullable: true),
                    ElectricConnection = table.Column<string>(nullable: true),
                    WaterConnectionType = table.Column<string>(nullable: true),
                    SolidWasteCollectionType = table.Column<string>(nullable: true),
                    DateSaved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileAsset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GISFileHolder",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileNumber = table.Column<string>(nullable: true),
                    CreationDate = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileHolder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GISFileInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceDate = table.Column<string>(nullable: true),
                    InvoiceAmount = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    isReversal = table.Column<string>(nullable: true),
                    InvoiceID = table.Column<string>(nullable: true),
                    DateSaved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GISFileInvoiceItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(nullable: false),
                    RevenueHeadId = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    DateSaved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileInvoiceItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GISFileParty",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyExtID = table.Column<string>(nullable: true),
                    PartyID = table.Column<string>(nullable: true),
                    PartyTitle = table.Column<string>(nullable: true),
                    PartyFirstName = table.Column<string>(nullable: true),
                    PartyLastName = table.Column<string>(nullable: true),
                    PartyMiddleName = table.Column<string>(nullable: true),
                    PartyFullName = table.Column<string>(nullable: true),
                    PartyType = table.Column<string>(nullable: true),
                    PartyGender = table.Column<string>(nullable: true),
                    PartyDOB = table.Column<string>(nullable: true),
                    PartyTIN = table.Column<string>(nullable: true),
                    PartyNIN = table.Column<string>(nullable: true),
                    PartyPhone1 = table.Column<string>(nullable: true),
                    PartyPhone2 = table.Column<string>(nullable: true),
                    PartyEmail = table.Column<string>(nullable: true),
                    PartyNationality = table.Column<string>(nullable: true),
                    PartyMaritalStatus = table.Column<string>(nullable: true),
                    PartyOccupation = table.Column<string>(nullable: true),
                    ContactAddress = table.Column<string>(nullable: true),
                    PartyRelation = table.Column<string>(nullable: true),
                    AcquisitionDate = table.Column<string>(nullable: true),
                    DateSaved = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GISFileParty", x => x.Id);
                });

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GISFileAssessment");

            migrationBuilder.DropTable(
                name: "GISFileAssessmentItem");

            migrationBuilder.DropTable(
                name: "GISFileAsset");

            migrationBuilder.DropTable(
                name: "GISFileHolder");

            migrationBuilder.DropTable(
                name: "GISFileInvoice");

            migrationBuilder.DropTable(
                name: "GISFileInvoiceItem");

            migrationBuilder.DropTable(
                name: "GISFileParty");

        }
    }
}
