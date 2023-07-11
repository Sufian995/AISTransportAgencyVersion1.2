using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIS_Transport_Agency.Migrations
{
    /// <inheritdoc />
    public partial class addingslotmodeltodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "ShoppingCart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_BookingId",
                table: "ShoppingCart",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_SlotBooking_BookingId",
                table: "ShoppingCart",
                column: "BookingId",
                principalTable: "SlotBooking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_SlotBooking_BookingId",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_BookingId",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "ShoppingCart");
        }
    }
}
