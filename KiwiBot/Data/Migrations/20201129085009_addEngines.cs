using Microsoft.EntityFrameworkCore.Migrations;

namespace KiwiBot.Data.Migrations
{
    public partial class addEngines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiCompatible",
                table: "Boorus");

            migrationBuilder.DropColumn(
                name: "FileUrlKey",
                table: "Boorus");

            migrationBuilder.DropColumn(
                name: "TagsKey",
                table: "Boorus");

            migrationBuilder.AddColumn<int>(
                name: "EngineId",
                table: "Boorus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Engines",
                columns: table => new
                {
                    EngineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EngineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagsKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrlKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engines", x => x.EngineId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boorus_EngineId",
                table: "Boorus",
                column: "EngineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boorus_Engines_EngineId",
                table: "Boorus",
                column: "EngineId",
                principalTable: "Engines",
                principalColumn: "EngineId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boorus_Engines_EngineId",
                table: "Boorus");

            migrationBuilder.DropTable(
                name: "Engines");

            migrationBuilder.DropIndex(
                name: "IX_Boorus_EngineId",
                table: "Boorus");

            migrationBuilder.DropColumn(
                name: "EngineId",
                table: "Boorus");

            migrationBuilder.AddColumn<bool>(
                name: "ApiCompatible",
                table: "Boorus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FileUrlKey",
                table: "Boorus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagsKey",
                table: "Boorus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
