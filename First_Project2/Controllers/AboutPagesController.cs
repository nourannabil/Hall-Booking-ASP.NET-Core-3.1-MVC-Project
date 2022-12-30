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
using Microsoft.AspNetCore.Authorization;

namespace First_Project2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AboutPagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AboutPagesController(ModelContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: AboutPages
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.AboutPages.Include(a => a.Hall).Include(a => a.Home).Include(a => a.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: AboutPages/Details/5
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

            var aboutPage = await _context.AboutPages
                .Include(a => a.Hall)
                .Include(a => a.Home)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutPage == null)
            {
                return NotFound();
            }

            return View(aboutPage);
        }

        // GET: AboutPages/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id");
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: AboutPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagePath,Title,Description1,Description2,HomeId,HallId,UserId,ImageFile")] AboutPage aboutPage)
        {
            if (ModelState.IsValid)
            {
                if (aboutPage.ImageFile != null)
                {
                    string wwwrootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutPage.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await aboutPage.ImageFile.CopyToAsync(filestream);
                    }
                    aboutPage.ImagePath = fileName;
                    _context.Add(aboutPage);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", aboutPage.HallId);
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", aboutPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", aboutPage.UserId);
            return View(aboutPage);
        }

        // GET: AboutPages/Edit/5
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

            var aboutPage = await _context.AboutPages.FindAsync(id);
            if (aboutPage == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", aboutPage.HallId);
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", aboutPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", aboutPage.UserId);
            return View(aboutPage);
        }

        // POST: AboutPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImagePath,Title,Description1,Description2,HomeId,HallId,UserId,ImageFile")] AboutPage aboutPage)
        {
            if (id != aboutPage.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (aboutPage.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + aboutPage.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await aboutPage.ImageFile.CopyToAsync(filestream);
                        }
                        aboutPage.ImagePath = fileName;
                    }
                    _context.Update(aboutPage);
                    await _context.SaveChangesAsync();
                }
              
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutPageExists(aboutPage.Id))
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
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", aboutPage.HallId);
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", aboutPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", aboutPage.UserId);
            return View(aboutPage);
        }

        // GET: AboutPages/Delete/5
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

            var aboutPage = await _context.AboutPages
                .Include(a => a.Hall)
                .Include(a => a.Home)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutPage == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: AboutPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var aboutPage = await _context.AboutPages.FindAsync(id);
            _context.AboutPages.Remove(aboutPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutPageExists(decimal id)
        {
            return _context.AboutPages.Any(e => e.Id == id);
        }
    }
}
