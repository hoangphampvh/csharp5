using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMC5.Migrations
{
    public partial class jwt_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IsCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tokens_IdUser",
                table: "tokens",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");
        }
    }
}
