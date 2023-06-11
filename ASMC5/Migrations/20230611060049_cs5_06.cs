using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMC5.Migrations
{
    public partial class cs5_06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Cart_ProductID",
                table: "CartDetail");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_UserID",
                table: "CartDetail",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Cart_UserID",
                table: "CartDetail",
                column: "UserID",
                principalTable: "Cart",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Cart_UserID",
                table: "CartDetail");

            migrationBuilder.DropIndex(
                name: "IX_CartDetail_UserID",
                table: "CartDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Cart_ProductID",
                table: "CartDetail",
                column: "ProductID",
                principalTable: "Cart",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
