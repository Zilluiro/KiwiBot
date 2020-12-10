using Microsoft.EntityFrameworkCore.Migrations;

namespace KiwiBot.Data.Migrations
{
    public partial class LockedMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LockedMode",
                table: "Boorus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedMode",
                table: "Boorus");
        }
    }
}
