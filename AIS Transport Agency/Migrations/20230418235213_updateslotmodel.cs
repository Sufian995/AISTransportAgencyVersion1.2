using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIS_Transport_Agency.Migrations
{
    /// <inheritdoc />
    public partial class updateslotmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnPromotion",
                table: "SlotBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnPromotion",
                table: "SlotBooking");
        }
    }
}
