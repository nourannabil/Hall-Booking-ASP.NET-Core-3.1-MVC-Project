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
    public class FeaturesController : Controller
    {
        private readonly ModelContext _context;

        public FeaturesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Features
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.Features.Include(f => f.Category).Include(f => f.Hall).Include(f => f.Photo);
            return View(await modelContext.ToListAsync());
        }

        // GET: Features/Details/5
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

            var feature = await _context.Features
                .Include(f => f.Category)
                .Include(f => f.Hall)
                .Include(f => f.Photo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }


        /// <summary>
        /// For casscading List in create View
        /// </summary>
        public JsonResult Category()
        {
            var cat = _context.CategoryHalls.ToList();
            return new JsonResult(cat);
        }

        public JsonResult Hall(int id)
        {
            var hal = _context.Halls.Where(x => x.CategoryId == id).ToList();
            return new JsonResult(hal);
        }


        // GET: Features/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name");
            ViewData["PhotoId"] = new SelectList(_context.HallPhotos, "Id", "Id");
            return View();
        }

        // POST: Features/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Feat,CategoryId,HallId,PhotoId")] Feature feature)
        {

            if (ModelState.IsValid)
            {
                _context.Add(feature);
                await _context.SaveChangesAsync();
                return RedirectToAction("SeeFeature", new { Id = feature.HallId });
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", feature.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", feature.HallId);
            ViewData["PhotoId"] = new SelectList(_context.HallPhotos, "Id", "Id", feature.PhotoId);
            return View(feature);
        }

        // GET: Features/Edit/5
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

            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName", feature.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", feature.HallId);
            ViewData["PhotoId"] = new SelectList(_context.HallPhotos, "Id", "Id", feature.PhotoId);
            return View(feature);
        }

        // POST: Features/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Feat,CategoryId,HallId,PhotoId")] Feature feature)
        {
            if (id != feature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureExists(feature.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SeeFeature", new { Id = feature.HallId });
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", feature.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", feature.HallId);
            ViewData["PhotoId"] = new SelectList(_context.HallPhotos, "Id", "Id", feature.PhotoId);
            return View(feature);
        }

        // GET: Features/Delete/5
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

            var feature = await _context.Features
                .Include(f => f.Category)
                .Include(f => f.Hall)
                .Include(f => f.Photo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // POST: Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var feature = await _context.Features.FindAsync(id);
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction("SeeFeature", new { Id = feature.HallId });
        }

        private bool FeatureExists(decimal id)
        {
            return _context.Features.Any(e => e.Id == id);
        }



        ////////////////////////////////////////////////////////////////

        public IActionResult SeeFeature(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var feat = from item in _context.Features
                       where (item.Hall.Id == id)
                       select item;

            ViewBag.feat = feat;

            return View(feat);
        }


    }
}
