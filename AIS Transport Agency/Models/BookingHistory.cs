using System.ComponentModel.DataAnnotations;

namespace AIS_Transport_Agency.Models
{
    public class BookingHistory
    {
        [Key]
        public int Id { get; set; }
        public AppUser? User { get; set; }
        public string? UserId { get; set; }
        public SlotBooking? Booking { get; set; }
        public int? BookingId { get; set; }

        public int IsConfirmed { get; set; }
    }
}
