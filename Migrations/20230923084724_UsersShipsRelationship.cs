using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace battleship_server.Migrations
{
    /// <inheritdoc />
    public partial class UsersShipsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipUser",
                columns: table => new
                {
                    OwnedShipsID = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipUser", x => new { x.OwnedShipsID, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ShipUser_Ships_OwnedShipsID",
                        column: x => x.OwnedShipsID,
                        principalTable: "Ships",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShipUser_UsersId",
                table: "ShipUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipUser");
        }
    }
}
