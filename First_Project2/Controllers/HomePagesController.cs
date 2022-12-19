using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using First_Project2.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace First_Project2.Controllers
{
    public class HomePagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomePagesController(ModelContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: HomePages
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            return View(await _context.HomePages.ToListAsync());
        }

        // GET: HomePages/Details/5
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
            var homePage = await _context.HomePages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // GET: HomePages/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            return View();
        }

        // POST: HomePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagePath,Title,Description1,Description2,ImageFile")] HomePage homePage)
        {
            if (ModelState.IsValid)
            {
                if (homePage.ImageFile != null)
                {
                    string wwwrootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + homePage.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await homePage.ImageFile.CopyToAsync(filestream);
                    }
                    homePage.ImagePath = fileName;
                    _context.Add(homePage);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(homePage);
        }

        // GET: HomePages/Edit/5
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

            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
            {
                return NotFound();
            }
            return View(homePage);
        }

        // POST: HomePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImagePath,Title,Description1,Description2,ImageFile")] HomePage homePage)
        {
            if (id != homePage.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (homePage.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + homePage.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await homePage.ImageFile.CopyToAsync(filestream);
                        }
                        homePage.ImagePath = fileName;
                    }
                    _context.Update(homePage);
                    await _context.SaveChangesAsync();
                }
            
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageExists(homePage.Id))
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
            return View(homePage);
        }

        // GET: HomePages/Delete/5
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

            var homePage = await _context.HomePages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: HomePages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var homePage = await _context.HomePages.FindAsync(id);
            _context.HomePages.Remove(homePage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomePageExists(decimal id)
        {
            return _context.HomePages.Any(e => e.Id == id);
        }
    }
}
