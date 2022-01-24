using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CbgSite.Migrations
{
    public partial class linksdescr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkGithub",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkLinkedIn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkTwitter",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    CbgUserId = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkGithub",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkLinkedIn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkTwitter",
                table: "AspNetUsers");
        }
    }
}
