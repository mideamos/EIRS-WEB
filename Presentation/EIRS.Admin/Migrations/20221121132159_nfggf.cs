using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EIRS.Admin.Migrations
{
    public partial class nfggf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "AssessmentRuleRollover",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentRuleID = table.Column<int>(nullable: false),
                    AssessmentRuleCode = table.Column<string>(nullable: true),
                    Profileid = table.Column<int>(nullable: false),
                    AssessmentRuleName = table.Column<string>(nullable: true),
                    AssessmentAmount = table.Column<decimal>(nullable: false),
                    Taxyear = table.Column<int>(nullable: false),
                    RuleRunId = table.Column<int>(nullable: false),
                    Paymentfrequencyid = table.Column<int>(nullable: false),
                    Active = table.Column<string>(nullable: true),
                    Createdby = table.Column<int>(nullable: false),
                    ARAIID = table.Column<int>(nullable: false),
                    AssessmentItemID = table.Column<int>(nullable: false),
                    Createddate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentRuleRollover", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentRuleRollover");
        }
    }
}
