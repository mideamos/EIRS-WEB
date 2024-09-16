using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EIRS.Web.Migrations
{
    public partial class newnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSvaed",
                table: "GISFileHolder");

            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileParty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileInvoice",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSaved",
                table: "GISFileHolder",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "GISFileHolder",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileHolder",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileAsset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageNo",
                table: "GISFileAssessment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileParty");

            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileInvoice");

            migrationBuilder.DropColumn(
                name: "DateSaved",
                table: "GISFileHolder");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "GISFileHolder");

            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileHolder");

            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileAsset");

            migrationBuilder.DropColumn(
                name: "PageNo",
                table: "GISFileAssessment");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSvaed",
                table: "GISFileHolder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
