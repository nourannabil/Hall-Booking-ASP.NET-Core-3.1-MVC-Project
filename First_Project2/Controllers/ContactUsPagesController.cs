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
    public class ContactUsPagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ContactUsPagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: ContactUsPages
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.ContactUsPages.Include(c => c.Home).Include(c => c.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: ContactUsPages/Details/5
        [Authorize(Roles = "Admin")]
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

            var contactUsPage = await _context.ContactUsPages
                .Include(c => c.Home)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUsPage == null)
            {
                return NotFound();
            }

            return View(contactUsPage);
        }

        // GET: ContactUsPages/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id");
            return View();
        }

        // POST: ContactUsPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagePath,Title,Description1,Description2,Lname,Fname,PhoneNumber,Email,Message,Map,HomeId,UserId,ImageFile")] ContactUsPage contactUsPage)
        {
            if (ModelState.IsValid)
            {
                if (contactUsPage.ImageFile != null)
                {
                    string wwwrootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + contactUsPage.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await contactUsPage.ImageFile.CopyToAsync(filestream);
                    }
                    contactUsPage.ImagePath = fileName;
                    _context.Add(contactUsPage);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", contactUsPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", contactUsPage.UserId);
            return View(contactUsPage);
        }
        

        // GET: ContactUsPages/Edit/5
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

            var contactUsPage = await _context.ContactUsPages.FindAsync(id);
            if (contactUsPage == null)
            {
                return NotFound();
            }
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", contactUsPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", contactUsPage.UserId);
            return View(contactUsPage);
        }

        // POST: ContactUsPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImagePath,Title,Description1,Description2,Lname,Fname,PhoneNumber,Email,Message,Map,HomeId,UserId,ImageFile")] ContactUsPage contactUsPage)
        {
            if (id != contactUsPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (contactUsPage.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + contactUsPage.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await contactUsPage.ImageFile.CopyToAsync(filestream);
                        }
                        contactUsPage.ImagePath = fileName;
                    }
                    _context.Update(contactUsPage);
                    await _context.SaveChangesAsync();
                }
                
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactUsPageExists(contactUsPage.Id))
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
            ViewData["HomeId"] = new SelectList(_context.HomePages, "Id", "Id", contactUsPage.HomeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Id", "Id", contactUsPage.UserId);
            return View(contactUsPage);
        }

        // GET: ContactUsPages/Delete/5
        [Authorize(Roles = "Admin")]
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

            var contactUsPage = await _context.ContactUsPages
                .Include(c => c.Home)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUsPage == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: ContactUsPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var contactUsPage = await _context.ContactUsPages.FindAsync(id);
            _context.ContactUsPages.Remove(contactUsPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactUsPageExists(decimal id)
        {
            return _context.ContactUsPages.Any(e => e.Id == id);
        }
    }
}
