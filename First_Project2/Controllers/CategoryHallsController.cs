using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project2.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace First_Project2.Controllers
{
    public class CategoryHallsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryHallsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: CategoryHalls
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            return View(await _context.CategoryHalls.ToListAsync());
        }

        // GET: CategoryHalls/Details/5
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

            var categoryHall = await _context.CategoryHalls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryHall == null)
            {
                return NotFound();
            }

            return View(categoryHall);
        }

        // GET: CategoryHalls/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            return View();
        }

        // POST: CategoryHalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,ImagePath,ImageFile")] CategoryHall categoryHall)
        {
            if (ModelState.IsValid)
            {
                if (categoryHall.ImageFile != null)
                {
                    string wwwrootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + categoryHall.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await categoryHall.ImageFile.CopyToAsync(filestream);
                    }
                    categoryHall.ImagePath = fileName;
                    _context.Add(categoryHall);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(categoryHall);
        }

        // GET: CategoryHalls/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            if (id == null)
            {
                return NotFound();
            }

            var categoryHall = await _context.CategoryHalls.FindAsync(id);
            if (categoryHall == null)
            {
                return NotFound();
            }
            return View(categoryHall);
        }

        // POST: CategoryHalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,CategoryName,ImagePath,ImageFile")] CategoryHall categoryHall)
        {
            if (id != categoryHall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (categoryHall.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + categoryHall.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await categoryHall.ImageFile.CopyToAsync(filestream);
                        }
                        categoryHall.ImagePath = fileName;
                    }
                    _context.Update(categoryHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryHallExists(categoryHall.Id))
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
            return View(categoryHall);
        }

        // GET: CategoryHalls/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var categoryHall = await _context.CategoryHalls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryHall == null)
            {
                return NotFound();
            }

            //return View(categoryHall);
            return RedirectToAction("Index");

        }

        // POST: CategoryHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var categoryHall = await _context.CategoryHalls.FindAsync(id);
            _context.CategoryHalls.Remove(categoryHall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CategoryHallExists(decimal id)
        {
            return _context.CategoryHalls.Any(e => e.Id == id);
        }

    }
}
