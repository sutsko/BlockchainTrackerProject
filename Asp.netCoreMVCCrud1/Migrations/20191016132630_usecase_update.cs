using Microsoft.EntityFrameworkCore.Migrations;

namespace Asp.netCoreMVCCrud1.Migrations
{
    public partial class usecase_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UsecaseName",
                table: "Usecases",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UsecaseName",
                table: "Usecases",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
