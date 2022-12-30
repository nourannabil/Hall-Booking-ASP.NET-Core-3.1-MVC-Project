using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace First_Project2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly ModelContext _context;

        public StatisticsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Statistics
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");

            ViewBag.TotalUserEnter = modelContext2.Count();

            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

            if(_context.Statistics != null)
            {
                var statics = _context.Statistics.FirstOrDefault();
                ViewBag.Static = statics;
            }

            var modelContext = _context.Statistics.Include(s => s.Booking).Include(s => s.Pay).Include(s => s.User);

            return View(await modelContext.ToListAsync());
        }

        // GET: Statistics/Details/5
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

            var statistic = await _context.Statistics
                .Include(s => s.Booking)
                .Include(s => s.Pay)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statistic == null)
            {
                return NotFound();
            }

            return View(statistic);
        }

        // GET: Statistics/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");

            ViewBag.TotalUserEnter = modelContext2.Count();

            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);


            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id");
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: Statistics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDay,EndDay,TotBookingNum,TotUsersRegisteredNum,Revenues,Expenses,BookingId,PayId,UserId")] Statistic statistic)
        {

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");

            ViewBag.TotalUserEnter = modelContext2.Count();

            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);


            if (ModelState.IsValid)
            {
                _context.Add(statistic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", statistic.BookingId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", statistic.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", statistic.UserId);
            return View(statistic);
        }

        // GET: Statistics/Edit/5
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

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");

            ViewBag.TotalUserEnter = modelContext2.Count();

            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

            var statistic = await _context.Statistics.FindAsync(id);

            if (statistic == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", statistic.BookingId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", statistic.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", statistic.UserId);
            return View(statistic);
        }

        // POST: Statistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,StartDay,EndDay,TotBookingNum,TotUsersRegisteredNum,Revenues,Expenses,BookingId,PayId,UserId")] Statistic statistic)
        {
            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");

            ViewBag.TotalUserEnter = modelContext2.Count();

            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

            if (id != statistic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statistic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatisticExists(statistic.Id))
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
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", statistic.BookingId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", statistic.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", statistic.UserId);
            return View(statistic);
        }

        // GET: Statistics/Delete/5
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

            var statistic = await _context.Statistics
                .Include(s => s.Booking)
                .Include(s => s.Pay)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statistic == null)
            {
                return NotFound();
            }

            return View(statistic);
        }

        // POST: Statistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var statistic = await _context.Statistics.FindAsync(id);
            _context.Statistics.Remove(statistic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatisticExists(decimal id)
        {
            return _context.Statistics.Any(e => e.Id == id);
        }
    }
}
