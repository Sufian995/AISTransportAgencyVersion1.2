using Microsoft.AspNetCore.Identity;

namespace AIS_Transport_Agency.Models
{
    public class BookingUser
    {
        public AppUser? User { get; set; }
        public string? UserId { get; set; }
        public SlotBooking? Booking { get; set; }
        public int? BookingId { get; set; }
    }
}
