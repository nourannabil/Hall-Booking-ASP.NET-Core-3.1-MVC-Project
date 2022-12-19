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
    public class TestimonialPagesController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialPagesController(ModelContext context)
        {
            _context = context;
        }

        // GET: TestimonialPages
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.TestimonialPages.Include(t => t.Home).Include(t => t.User);

            return View(await modelContext.ToListAsync());
        }

        // GET: TestimonialPages/Details/5
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

            var testimonialPage = await _context.TestimonialPages
                .Include(t => t.Home)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonialPage == null)
            {
                return NotFound();
            }

            return View(testimonialPage);
        }

        // GET: TestimonialPages/Create
        public IActionResult Create(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            var User = _context.UserInfos.SingleOrDefault(x => x.Id == id);
            ViewBag.user = User;

            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: TestimonialPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id , [Bind("Id,Opinion,HomeId,UserId")] TestimonialPage testimonialPage)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientUserName = HttpContext.Session.GetString("UserName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            var User = _context.UserInfos.SingleOrDefault(x => x.Id == id);
            ViewBag.user = User;

            if (ModelState.IsValid)
            {
                _context.Add(testimonialPage);
                await _context.SaveChangesAsync();
                return RedirectToAction("UpdateOpinion", "TestimonialPages", new {Id = testimonialPage.Id });
            }
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", testimonialPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", testimonialPage.UserId);
            return View(testimonialPage);
        }


        /// ///////////////////////////////////////////////////////////////


        public async Task<IActionResult> UpdateOpinion(int id)
        {
            using (var dbs = new ModelContext())
            {
                ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

                var test = dbs.TestimonialPages.SingleOrDefault(x => x.Id == id);
                ViewBag.test = test;

                string status = "UnApproved";

                test.Opinion = status + " " + test.Opinion;

                dbs.Update(test);
                await dbs.SaveChangesAsync();
                return RedirectToAction("Users", "Dashboard", new {Id = test.UserId});
            }
        }                

        //////////////////////////////////////////////////////////////////////////

        // GET: TestimonialPages/Edit/5
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

            var testimonialPage = await _context.TestimonialPages.FindAsync(id);
            if (testimonialPage == null)
            {
                return NotFound();
            }
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", testimonialPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Fname", testimonialPage.UserId);

            return View(testimonialPage);
        }

        // POST: TestimonialPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Opinion,HomeId,UserId")] TestimonialPage testimonialPage)
        {
            if (id != testimonialPage.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonialPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialPageExists(testimonialPage.Id))
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
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", testimonialPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", testimonialPage.UserId);
            return View(testimonialPage);
        }

        // GET: TestimonialPages/Delete/5
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

            var testimonialPage = await _context.TestimonialPages
                .Include(t => t.Home)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonialPage == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: TestimonialPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var testimonialPage = await _context.TestimonialPages.FindAsync(id);
            _context.TestimonialPages.Remove(testimonialPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialPageExists(decimal id)
        {
            return _context.TestimonialPages.Any(e => e.Id == id);
        }
    }
}
