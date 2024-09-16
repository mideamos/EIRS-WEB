using Microsoft.EntityFrameworkCore.Migrations;

namespace EIRS.Web.Migrations
{
    public partial class gisfilesjj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "GISFileParty",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "GISFileInvoice",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "GISFileAsset",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "GISFileAssessment",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "GISFileParty");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "GISFileInvoice");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "GISFileAsset");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "GISFileAssessment");
        }
    }
}
