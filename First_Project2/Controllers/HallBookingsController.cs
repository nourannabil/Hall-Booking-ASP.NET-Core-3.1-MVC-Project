using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project2.Models;
using Microsoft.AspNetCore.Http;

namespace First_Project2.Controllers
{
    public class HallBookingsController : Controller
    {
        private readonly ModelContext _context;

        public HallBookingsController(ModelContext context)
        {
            _context = context;
        }

        // GET: HallBookings
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.HallBookings.Include(h => h.Category).Include(h => h.Hall).Include(h => h.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: HallBookings/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var hallBooking = await _context.HallBookings
                .Include(h => h.Category)
                .Include(h => h.Hall)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hallBooking == null)
            {
                return NotFound();
            }

            return View(hallBooking);
        }

        // GET: HallBookings/Create
        public IActionResult Create(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientUserName = HttpContext.Session.GetString("UserName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var Booking = from item in _context.HallBookings where item.HallId == id select item;
            ViewBag.Booking = Booking;

            var hall = from item in _context.Halls
                       where (item.Id == id)
                       select item;
            ViewBag.HInfo = hall;

            var hally = _context.Halls.FirstOrDefault(x => x.Id == id);

            var category = from item in _context.CategoryHalls
                           where (item.Id == hally.CategoryId)
                           select item;
            ViewBag.CatInfo = category;


            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }



        // POST: HallBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingDate,BookingTime,CategoryId,HallId,UserId")] HallBooking hallBooking, int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientUserName = HttpContext.Session.GetString("UserName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            var Booking = from item in _context.HallBookings where item.HallId == id select item;
            ViewBag.Booking = Booking;

            var hall = from item in _context.Halls
                       where (item.Id == id)
                       select item;
            ViewBag.HInfo = hall;

            var hally = _context.Halls.FirstOrDefault(x => x.Id == id);

            var category = from item in _context.CategoryHalls
                           where (item.Id == hally.CategoryId)
                           select item;
            ViewBag.CatInfo = category;


            var list = new List<HallBooking>();

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item2 in Booking)
                    {
                        if (hallBooking.BookingDate > item2.BookingDate || hallBooking.BookingDate < item2.BookingDate)
                        {
                            list.Add(hallBooking);
                        }
                        else
                        {
                            ViewData["Message"] = "Please Select Another Day, This Day Not Available";
                            return View();
                        }

                    }

                    if (list.Count > 0 )
                    {
                        await _context.HallBookings.AddRangeAsync(list);
                        await _context.SaveChangesAsync();

                        var LastId = _context.HallBookings.OrderByDescending(p => p.Id).FirstOrDefault().Id;

                        Request request = new Request();
                        request.Status = "Pending";
                        request.CategoryId = hallBooking.CategoryId;
                        request.HallId = hallBooking.HallId;
                        request.UserId = hallBooking.UserId;
                        request.BookingId = LastId;
                        _context.Requests.Add(request);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("MyBooking", "Dashboard", new { Id = hallBooking.UserId });
                    }

                    if (list.Count == 0) 
                    {
                        _context.Add(hallBooking);
                        await _context.SaveChangesAsync();

                        var LastId = _context.HallBookings.OrderByDescending(p => p.Id).FirstOrDefault().Id;

                        Request request = new Request();
                        request.Status = "Pending";
                        request.CategoryId = hallBooking.CategoryId;
                        request.HallId = hallBooking.HallId;
                        request.UserId = hallBooking.UserId;
                        request.BookingId = LastId;
                        _context.Requests.Add(request);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("MyBooking", "Dashboard", new { Id = hallBooking.UserId });
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return View(hallBooking);
        }

        // GET: HallBookings/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var hallBooking = await _context.HallBookings.FindAsync(id);
            if (hallBooking == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName", hallBooking.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", hallBooking.HallId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", hallBooking.UserId);
            return View(hallBooking);
        }

        // POST: HallBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,BookingDate,BookingTime,CategoryId,HallId,UserId")] HallBooking hallBooking)
        {
            if (id != hallBooking.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hallBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallBookingExists(hallBooking.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", hallBooking.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", hallBooking.HallId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", hallBooking.UserId);
            return View(hallBooking);
        }

        // GET: HallBookings/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            var hallBooking = await _context.HallBookings
                .Include(h => h.Category)
                .Include(h => h.Hall)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hallBooking == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: HallBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var hallBooking = await _context.HallBookings.FindAsync(id);
            _context.HallBookings.Remove(hallBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallBookingExists(decimal id)
        {
            return _context.HallBookings.Any(e => e.Id == id);
        }

    }

}
