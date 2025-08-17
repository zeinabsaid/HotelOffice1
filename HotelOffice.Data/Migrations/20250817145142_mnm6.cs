using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelOffice.Data.Migrations
{
    /// <inheritdoc />
    public partial class mnm6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Rooms");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Rooms",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Rooms",
                type: "TEXT",
                nullable: true);
        }
    }
}
