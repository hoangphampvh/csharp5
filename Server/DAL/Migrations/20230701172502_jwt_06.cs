using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMC5.Migrations
{
    public partial class jwt_06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreateBy",
                table: "Product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Product");
        }
    }
}
