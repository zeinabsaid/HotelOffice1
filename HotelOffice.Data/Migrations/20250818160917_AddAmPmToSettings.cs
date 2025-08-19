using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelOffice.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAmPmToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckInAmPm",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CheckOutAmPm",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInAmPm",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CheckOutAmPm",
                table: "Settings");
        }
    }
}
