using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelOffice.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAmPmToSettings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInHour",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CheckOutHour",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "CheckOutAmPm",
                table: "Settings",
                newName: "CheckOutTime");

            migrationBuilder.RenameColumn(
                name: "CheckInAmPm",
                table: "Settings",
                newName: "CheckInTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOutTime",
                table: "Settings",
                newName: "CheckOutAmPm");

            migrationBuilder.RenameColumn(
                name: "CheckInTime",
                table: "Settings",
                newName: "CheckInAmPm");

            migrationBuilder.AddColumn<int>(
                name: "CheckInHour",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckOutHour",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
