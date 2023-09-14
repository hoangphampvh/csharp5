using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMC5.Migrations
{
    public partial class jwt_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tokens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tokens");
        }
    }
}
