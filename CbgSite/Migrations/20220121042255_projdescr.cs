using Microsoft.EntityFrameworkCore.Migrations;

namespace CbgSite.Migrations
{
    public partial class projdescr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionLong",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkGithub",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkTwitter",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionLong",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LinkGithub",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LinkTwitter",
                table: "Projects");
        }
    }
}
