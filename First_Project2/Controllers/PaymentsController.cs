using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace First_Project2.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ModelContext _context;

        public PaymentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            return View(await modelContext.ToListAsync());
        }

        // GET: Payments/Details/5
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

            var payment = await _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.Card)
                .Include(p => p.Hall)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var BookingInfo = _context.HallBookings.FirstOrDefault(m => m.Id == id);
            ViewBag.BookingInfo = BookingInfo;

            var userInfo = from item in _context.UserInfos where item.Id == BookingInfo.UserId select item;
            ViewBag.userInfo = userInfo;

            var catInfo = from item in _context.CategoryHalls where item.Id == BookingInfo.CategoryId select item;
            ViewBag.CatInfo = catInfo;

            var hallInfo = from item in _context.Halls where item.Id == BookingInfo.HallId select item;
            ViewBag.HInfo = hallInfo;

            var card = from item in _context.Cards where item.UserId == BookingInfo.UserId select item;
            ViewBag.card = card;

            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id");
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "CardNumber");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,PayDate,CardId,HallId,BookingId,UserId")] Payment payment)
        {
            var BookingInfo = _context.HallBookings.FirstOrDefault(m => m.Id == id);
            ViewBag.BookingInfo = BookingInfo;

            var userInfo = from item in _context.UserInfos where item.Id == BookingInfo.UserId select item;
            ViewBag.userInfo = userInfo;

            var catInfo = from item in _context.CategoryHalls where item.Id == BookingInfo.CategoryId select item;
            ViewBag.CatInfo = catInfo;

            var hallInfo = from item in _context.Halls where item.Id == BookingInfo.HallId select item;
            ViewBag.HInfo = hallInfo;

            var card = from item in _context.Cards where item.UserId == BookingInfo.UserId select item;
            ViewBag.card = card;
            

            if (ModelState.IsValid)
            {

                foreach (var item in card)
                {
                    payment.CardId = item.Id;
                }

                _context.Add(payment);

                await _context.SaveChangesAsync();

                return RedirectToAction("UpdateBalance", "Payments", new { Id = id });

            }

            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", payment.BookingId);
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "CardNumber", payment.CardId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", payment.HallId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", payment.UserId);
            return View(payment);
        }


        public async Task<IActionResult> UpdateBalance(int id)
        {

            using (var dbs = new ModelContext())
            {
                ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
                ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
                ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
                ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

                var payment = dbs.Payments.SingleOrDefault(x => x.Id == id);
                ViewBag.pay = payment;

                var Hall = _context.Halls.SingleOrDefault(x => x.Id == payment.HallId); ;
                ViewBag.Hall = Hall;

                var card = dbs.Cards.Where(x => x.Id == payment.CardId).SingleOrDefault();
                ViewBag.card = card;

                var RequestPay = dbs.Requests.Where(x => x.BookingId == payment.BookingId).SingleOrDefault();
                ViewBag.RequestPay = RequestPay;

                if (Hall.Price > card.Balance)
                {
                    card.Balance = card.Balance;
                    dbs.Remove(payment);
                    dbs.SaveChanges();
                    return View();
                }
                else
                {
                    card.Balance = card.Balance - Hall.Price;

                    RequestPay.PayId = payment.Id;
                    dbs.Update(RequestPay);
                    dbs.SaveChanges();
                }

                dbs.Update(card);
                await dbs.SaveChangesAsync();
                return RedirectToAction("Bill", "Dashboard", new { Id = id });

            }
        }

        // GET: Payments/Edit/5
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

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", payment.BookingId);
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Cvv", payment.CardId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", payment.HallId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", payment.UserId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,PayDate,CardId,HallId,BookingId,UserId")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(payment);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
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
            ViewData["BookingId"] = new SelectList(_context.HallBookings, "Id", "Id", payment.BookingId);
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Cvv", payment.CardId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", payment.HallId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Delete/5
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

            var payment = await _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.Card)
                .Include(p => p.Hall)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(decimal id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
