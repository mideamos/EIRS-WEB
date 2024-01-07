using Microsoft.EntityFrameworkCore.Migrations;

namespace EIRS.Web.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                table: "GISFileParty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                table: "GISFileInvoiceItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                table: "GISFileInvoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                table: "GISFileAsset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                table: "GISFileAssessmentItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileNumber",
                table: "GISFileAssessment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileNumber",
                table: "GISFileParty");

            migrationBuilder.DropColumn(
                name: "FileNumber",
                table: "GISFileInvoiceItem");

            migrationBuilder.DropColumn(
                name: "FileNumber",
                table: "GISFileInvoice");

            migrationBuilder.DropColumn(
                name: "FileNumber",
                table: "GISFileAsset");

            migrationBuilder.DropColumn(
                name: "FileNumber",
                table: "GISFileAssessmentItem");

            migrationBuilder.DropColumn(
                name: "FileNumber",
                table: "GISFileAssessment");
        }
    }
}
