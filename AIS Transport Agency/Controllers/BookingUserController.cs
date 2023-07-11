using AIS_Transport_Agency.Data;
using AIS_Transport_Agency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIS_Transport_Agency.Controllers
{
    public class BookingUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingUserController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var userBookings = _context.BookingHistory.Include(x => x.User).Include(x => x.Booking).ToList();
                return View(userBookings);
            }
            else
            {
                var userBookings = _context.BookingHistory.Where(x => x.UserId == _userManager.GetUserId(User)).Include(x => x.User).Include(x => x.Booking).ToList();
                return View(userBookings);
            }
        }

        public async Task<IActionResult> Delete(string? userId, int? bookingId)
        {
            var userBooking = _context.BookingHistory.FirstOrDefault(x => x.UserId == userId && x.BookingId == bookingId);

            _context.BookingHistory.Remove(userBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
