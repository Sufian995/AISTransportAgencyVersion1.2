using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIS_Transport_Agency.Migrations
{
    /// <inheritdoc />
    public partial class updatedbcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    IsConfirmed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingHistory_SlotBooking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "SlotBooking",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_BookingId",
                table: "BookingHistory",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_UserId",
                table: "BookingHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingHistory");
        }
    }
}
