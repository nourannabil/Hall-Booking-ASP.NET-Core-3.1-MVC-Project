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
    public class RequestsController : Controller
    {
        private readonly ModelContext _context;

        public RequestsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Requests/Details/5
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

            var request = await _context.Requests
                .Include(r => r.Booking)
                .Include(r => r.Category)
                .Include(r => r.Hall)
                .Include(r => r.Pay)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            var BookDetails = from item in _context.HallBookings where item.Id == request.BookingId select item;
            ViewBag.Booking = BookDetails;

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name");
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,CategoryId,HallId,BookingId,PayId,UserId")] Request request)
        {

            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", request.BookingId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", request.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", request.HallId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", request.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", request.UserId);
            return View(request);
        }

        // GET: Requests/Edit/5
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

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", request.BookingId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName", request.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", request.HallId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", request.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", request.UserId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Status,CategoryId,HallId,BookingId,PayId,UserId")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { Id = id });
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", request.BookingId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", request.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", request.HallId);
            ViewData["PayId"] = new SelectList(_context.Payments, "Id", "Id", request.PayId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", request.UserId);
            return View(request);
        }

        // GET: Requests/Delete/5
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

            var request = await _context.Requests
                .Include(r => r.Booking)
                .Include(r => r.Category)
                .Include(r => r.Hall)
                .Include(r => r.Pay)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Search For Request 
        /// </summary>
        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            ViewBag.Name = "";
            ViewBag.Status = "";

            var modelContext = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            return View(modelContext.ToList());
        }

        [HttpPost]
        public IActionResult Search(string userName, string Status)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            if (userName == null && Status == null)
            {
                ViewBag.Name = userName;
                ViewBag.Status = Status;
                var modelContext = _context.Requests.ToList();

                return View(modelContext);
            }
            else if (userName != null && Status == null)
            {
                ViewBag.Name = userName;
                ViewBag.Status = Status;

                List<Request> requestList = new List<Request>();

                requestList = _context.Requests.Include(r => r.Booking)
                    .Include(r => r.Category)
                    .Include(r => r.Hall)
                    .Include(r => r.Pay)
                    .Include(r => r.User)
                    .Where(x => x.User.Fname.Contains(userName)).ToList();

                return View(requestList);

            }
            else if (userName == null && Status != null)
            {
                ViewBag.Name = userName;
                ViewBag.Status = Status;

                List<Request> requestList = new List<Request>();

                requestList = _context.Requests.Include(r => r.Booking)
                    .Include(r => r.Category)
                    .Include(r => r.Hall)
                    .Include(r => r.Pay)
                    .Include(r => r.User)
                    .Where(x => x.Status.StartsWith(Status)).ToList();

                return View(requestList);
            }
            else
            {
                ViewBag.Name = userName;
                ViewBag.Status = Status;

                List<Request> requestList = new List<Request>();

                requestList = _context.Requests.Include(r => r.Booking)
                    .Include(r => r.Category)
                    .Include(r => r.Hall)
                    .Include(r => r.Pay)
                    .Include(r => r.User)
                    .Where(x => x.User.Fname.StartsWith(userName) && x.Status.StartsWith(Status)).ToList();

                return View(requestList);
            }
        }

        private bool RequestExists(decimal id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
