using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AIS_Transport_Agency.Data;
using AIS_Transport_Agency.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AIS_Transport_Agency.Controllers
{
    [Authorize]
    public class SlotBookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SlotBookingsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SlotBookings
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Index()
        {

              return _context.SlotBooking != null ? 
                          View(await _context.SlotBooking.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SlotBooking'  is null.");
        }

        public async Task<IActionResult> Apply(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var booking = _context.SlotBooking.Where(x => x.Id == id).Include(x => x.Users).FirstOrDefault();
            var bookingUser = _context.BookingUser.Where(x => x.BookingId == id);

            //Check database and get the amount of booked user, if its equals or higher, return to index page
            if(bookingUser.Count() >= booking.Slots) return RedirectToAction(nameof(Index));

            if (booking == null ) return RedirectToAction(nameof(Index));

            var user = await _userManager.GetUserAsync(User);

            BookingUser bookUser = new BookingUser();

            bookUser.UserId = user.Id;
            bookUser.BookingId = id;
           
            _context.BookingUser.Add(bookUser);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "BookingUser");
        }

        // GET: SlotBookings/Details/5
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SlotBooking == null)
            {
                return NotFound();
            }

            var slotBooking = await _context.SlotBooking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slotBooking == null)
            {
                return NotFound();
            }

            return View(slotBooking);
        }

        // GET: SlotBookings/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SlotBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Datetime,SlotName,OnPromotion,Slots,Price,LicenseType,ImageURL")] SlotBooking slotBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slotBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slotBooking);
        }

        // GET: SlotBookings/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SlotBooking == null)
            {
                return NotFound();
            }

            var slotBooking = await _context.SlotBooking.FindAsync(id);
            if (slotBooking == null)
            {
                return NotFound();
            }
            return View(slotBooking);
        }

        // POST: SlotBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Datetime,SlotName,OnPromotion,Slots,Price,LicenseType,ImageURL")] SlotBooking slotBooking)
        {
            if (id != slotBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slotBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlotBookingExists(slotBooking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slotBooking);
        }

        // GET: SlotBookings/Delete/5
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SlotBooking == null)
            {
                return NotFound();
            }

            var slotBooking = await _context.SlotBooking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slotBooking == null)
            {
                return NotFound();
            }

            return View(slotBooking);
        }

        // POST: SlotBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SlotBooking == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SlotBooking'  is null.");
            }
            var slotBooking = await _context.SlotBooking.FindAsync(id);
            if (slotBooking != null)
            {
                _context.SlotBooking.Remove(slotBooking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlotBookingExists(int id)
        {
          return (_context.SlotBooking?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
