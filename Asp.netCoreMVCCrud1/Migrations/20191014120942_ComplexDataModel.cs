using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asp.netCoreMVCCrud1.Migrations
{
    public partial class ComplexDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArticleHeadline",
                table: "Projects",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationName = table.Column<string>(nullable: true),
                    OrganizationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });
            migrationBuilder.Sql("INSERT INTO Organizations VALUES('Default', 0)");

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    SectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SectorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.SectorId);
                });
            migrationBuilder.Sql("INSERT INTO Sectors VALUES('Default') ");

            migrationBuilder.CreateTable(
                name: "Usecases",
                columns: table => new
                {
                    UsecaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UsecaseName = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usecases", x => x.UsecaseId);
                });
            migrationBuilder.Sql("INSERT INTO Usecases VALUES(1)");

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    IndustryId = table.Column<int>(nullable: false, defaultValue:0)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IndustryName = table.Column<string>(nullable: true),
                    IndustryDescription = table.Column<string>(nullable: true),
                    SectorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.IndustryId);
                    table.ForeignKey(
                        name: "FK_Industries_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "SectorId",
                        onDelete: ReferentialAction.Cascade);
                });

           // migrationBuilder.Sql("INSERT INTO Industries VALUES('Default', 'Default', 1) ");
           // migrationBuilder.Sql("INSERT INTO Projects VALUES('Default', 'Default', 'Default', DateTime.Now, 'FALSE', 1, 'default', 1, 1, 'default', 'default') ");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IndustryId",
                table: "Projects",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UseCaseId",
                table: "Projects",
                column: "UseCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_SectorId",
                table: "Industries",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Industries_IndustryId",
                table: "Projects",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "IndustryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organizations_OrganizationId",
                table: "Projects",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Usecases_UseCaseId",
                table: "Projects",
                column: "UseCaseId",
                principalTable: "Usecases",
                principalColumn: "UsecaseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Industries_IndustryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organizations_OrganizationId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Usecases_UseCaseId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Usecases");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_Projects_IndustryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UseCaseId",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleHeadline",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
