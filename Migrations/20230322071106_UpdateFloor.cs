using Microsoft.EntityFrameworkCore.Migrations;

namespace TV_DASH_API.Migrations
{
    public partial class UpdateFloor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "TVDash_Images",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "TVDash_Images");
        }
    }
}
