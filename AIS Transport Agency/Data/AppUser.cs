using Microsoft.AspNetCore.Identity;

namespace AIS_Transport_Agency.Models
{
    public class AppUser : IdentityUser
    {
        public List<SlotBooking>? SlotBookings { get; set; }
    }
}
