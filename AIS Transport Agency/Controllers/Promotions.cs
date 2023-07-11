using AIS_Transport_Agency.Data;
using AIS_Transport_Agency.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AIS_Transport_Agency.Controllers
{
    
    public class Promotions : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;

        public Promotions(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> manager, INotyfService notyfService)
        {
            _logger = logger;
            _context = context;
            _userManager = manager;
            _toastNotification = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var query = await _context.SlotBooking.Where(x => x.OnPromotion == 1).ToListAsync();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = _context.BookingUser.Where(u => u.UserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Apply(int? id)
        {

            var duplicate = _context.BookingUser.Where(x => x.UserId == _userManager.GetUserId(User) && x.BookingId == id).ToList();

            if (duplicate.Any())
            {
                _toastNotification.Error("There is a slot with same details in your Cart", 10);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (id == null) return RedirectToAction(nameof(Index));

                var booking = _context.SlotBooking.Where(x => x.Id == id).Include(x => x.Users).FirstOrDefault();
                var bookingUser = _context.BookingUser.Where(x => x.BookingId == id);

                //Check database and get the amount of booked user, if its equals or higher, return to index page
                if (bookingUser.Count() >= booking.Slots) return RedirectToAction(nameof(Index));

                if (booking == null) return RedirectToAction(nameof(Index));

                var user = await _userManager.GetUserAsync(User);

                BookingUser bookUser = new BookingUser();

                bookUser.UserId = user.Id;
                bookUser.BookingId = id;
                bookUser.IsConfirmed = 0;


                _context.BookingUser.Add(bookUser);

                await _context.SaveChangesAsync();

                var Count = _context.BookingUser.Where(c => c.UserId == _userManager.GetUserId(User)).ToList().Count();

                HttpContext.Session.SetInt32("ssCartCount", Count);

                return RedirectToAction("Index", "BookingCart");
            }

        }
    }
}
