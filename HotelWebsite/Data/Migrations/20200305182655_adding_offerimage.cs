using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelWebsite.Data.Migrations
{
    public partial class adding_offerimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableTo",
                table: "Offers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableFrom",
                table: "Offers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "OfferImages",
                columns: table => new
                {
                    OfferId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferImages", x => new { x.OfferId, x.ImagePath });
                    table.ForeignKey(
                        name: "FK_OfferImages_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableTo",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableFrom",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
