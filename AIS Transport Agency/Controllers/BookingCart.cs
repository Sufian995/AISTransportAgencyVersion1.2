using AIS_Transport_Agency.Data;
using AIS_Transport_Agency.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIS_Transport_Agency.Controllers
{
    [Authorize]
    public class BookingCart : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingCart(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var userBookings = _context.BookingUser.Include(x => x.User).Include(x => x.Booking).ToList();
                return View(userBookings);
            }
            else
            {
                var userBookings = _context.BookingUser.Where(x => x.UserId == _userManager.GetUserId(User)).Include(x => x.User).Include(x => x.Booking).ToList();
                return View(userBookings);
            }
        }


        public IActionResult PaymentOption(string? userId, int? bookingId)
        {
            var getitem = _context.BookingUser.Where(x => x.UserId == userId && x.BookingId == bookingId).FirstOrDefault();

            return View(getitem);
        }


        public IActionResult PayOnSite(string? userId, int? bookingId)
        {
            var getitem = _context.BookingUser.Where(x => x.UserId == userId && x.BookingId == bookingId).FirstOrDefault();

            if(getitem != null)
            {
                BookingHistory bkb = new BookingHistory();

                bkb.UserId= getitem.UserId;
                bkb.BookingId= getitem.BookingId;
                bkb.User = getitem.User;
                bkb.Booking = getitem.Booking;
                bkb.IsConfirmed= 1;

                _context.BookingUser.Remove(getitem);
                _context.BookingHistory.Add(bkb);

                _context.SaveChanges();

                return RedirectToAction("Index", "BookingUser");
            }

            return View(getitem);
        }


        public async Task<IActionResult> DeleteCart(string? userId, int? bookingId)
        {
            var userBooking = _context.BookingUser.FirstOrDefault(x => x.UserId == userId && x.BookingId == bookingId);

            _context.BookingUser.Remove(userBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
