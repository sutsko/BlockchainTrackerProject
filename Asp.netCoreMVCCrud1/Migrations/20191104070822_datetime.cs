using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asp.netCoreMVCCrud1.Migrations
{
    public partial class datetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ArticleDate",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArticleDate",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
