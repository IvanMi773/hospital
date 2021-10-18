using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Hospital.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var userBookings = _context
                .Booking
                .Where(booking => booking.UserId == userId)
                .Include(b => b.Doctor)
                .ToListAsync();
            return View(await userBookings);
        }

        public async Task<IActionResult> AdminIndex()
        {
            var applicationDbContext = _context.Booking.Include(b => b.Doctor);
            return View("Index", await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize(Roles = UserRole.User)]
        [HttpGet("/Booking/Create/{doctorId}")]
        public IActionResult Create(int? doctorId)
        {
            ViewData["DoctorId"] = doctorId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> Create([Bind("DoctorId,DateFrom,DateTo,Description")] Booking booking)
        {
            if (DateTime.Compare(booking.DateFrom, booking.DateTo) > 0)
            {
                ViewData["dateErr"] = "Date from is bigger than date to";
                return View(booking);
            }
            // Todo: send error
            await foreach (var item in _context.Booking)
            {
                if (item.DoctorId == booking.DoctorId)
                {
                    if (
                        DateTime.Compare(booking.DateFrom, item.DateFrom) >= 0 && DateTime.Compare(booking.DateFrom, item.DateTo) <= 0 || 
                        DateTime.Compare(booking.DateTo, item.DateFrom) <= 0 && DateTime.Compare(booking.DateTo, item.DateTo) >= 0
                    )
                    {
                        Console.WriteLine("booking date from: " + booking.DateFrom + " booking date to " + booking.DateTo);
                        Console.WriteLine("item date from: " + item.DateFrom + " item date to " + item.DateTo);
                        ViewData["dateErr"] = "Date between " + booking.DateFrom + " and " + booking.DateTo + " is already booked.";
                        return View(booking);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                booking.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(booking);
        }

        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Id", booking.DoctorId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,UserId,DateFrom,DateTo,Description")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctor, "Id", "Id", booking.DoctorId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
