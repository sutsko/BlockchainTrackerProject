using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asp.netCoreMVCCrud1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleHeadline = table.Column<string>(nullable: true),
                    ArticleUrl = table.Column<string>(nullable: true),
                    ArticleDescription = table.Column<string>(nullable: true),
                    ArticleDate = table.Column<string>(nullable: true),
                    Confidentiality = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: false, defaultValue: 0),
                    Country = table.Column<string>(nullable: true),
                    IndustryId = table.Column<int>(nullable: false, defaultValue: 0),
                    UseCaseId = table.Column<int>(nullable: false, defaultValue:0),
                    Maturity = table.Column<string>(nullable: true),
                    TechnicalVendor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
