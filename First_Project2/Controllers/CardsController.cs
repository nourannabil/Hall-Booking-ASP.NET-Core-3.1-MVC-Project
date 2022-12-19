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
    public class CardsController : Controller
    {
        private readonly ModelContext _context;

        public CardsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Cards
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.Cards.Include(c => c.User).Where(x => x.UserId == id);
            
            return View(await modelContext.ToListAsync());
        }

        // GET: Cards/Details/5
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

            var card = await _context.Cards.Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardNumber,NameOnCard,Cvv,Balance,CardType,ExpiryDate,UserId")] Card card)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new {Id = card.UserId});
            }
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", card.UserId);
            return View(card);
        }

        // GET: Cards/Edit/5
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

            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }
           
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", card.UserId);
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,CardNumber,NameOnCard,Cvv,Balance,CardType,ExpiryDate,UserId")] Card card)
        {
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { Id = card.UserId });
            }
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", card.UserId);
            return View(card);
        }

        // GET: Cards/Delete/5
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

            var card = await _context.Cards.Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", new { Id = card.UserId });
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { Id = card.UserId });
        }

        private bool CardExists(decimal id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
