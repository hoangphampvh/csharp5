using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMC5.Migrations
{
    public partial class jwt_05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Iaced",
                table: "tokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iaced",
                table: "tokens");
        }
    }
}
