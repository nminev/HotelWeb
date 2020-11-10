using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Data.Migrations
{
    public partial class removingReviewcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "Offers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
