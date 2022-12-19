using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project2.Models;

namespace First_Project2.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ModelContext _context;

        public ReportsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Reports.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Booking)
                .Include(r => r.Category)
                .Include(r => r.Hall)
                .Include(r => r.Pay)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id");
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportName,CategoryId,HallId,BookingId,PayId,UserId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", report.BookingId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", report.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", report.HallId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", report.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", report.UserId);
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", report.BookingId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", report.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", report.HallId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", report.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", report.UserId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ReportName,CategoryId,HallId,BookingId,PayId,UserId")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
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
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", report.BookingId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", report.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", report.HallId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", report.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", report.UserId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Booking)
                .Include(r => r.Category)
                .Include(r => r.Hall)
                .Include(r => r.Pay)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var report = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(decimal id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }


    }
}




