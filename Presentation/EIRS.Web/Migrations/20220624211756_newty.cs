using Microsoft.EntityFrameworkCore.Migrations;

namespace EIRS.Web.Migrations
{
    public partial class newty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileInvoiceItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileAssessmentItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileInvoiceItem");

            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileAssessmentItem");
        }
    }
}
