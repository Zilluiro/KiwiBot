using Microsoft.EntityFrameworkCore.Migrations;

namespace KiwiBot.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boorus",
                columns: table => new
                {
                    BooruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BooruName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiEndpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiCompatible = table.Column<bool>(type: "bit", nullable: false),
                    TagsKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrlKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boorus", x => x.BooruId);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    BooruId = table.Column<int>(type: "int", nullable: false),
                    ChatMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_Chats_Boorus_BooruId",
                        column: x => x.BooruId,
                        principalTable: "Boorus",
                        principalColumn: "BooruId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_BooruId",
                table: "Chats",
                column: "BooruId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Boorus");
        }
    }
}
