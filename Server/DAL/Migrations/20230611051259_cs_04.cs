using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMC5.Migrations
{
    public partial class cs_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartID",
                table: "CartDetail",
                newName: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "CartDetail",
                newName: "CartID");
        }
    }
}
