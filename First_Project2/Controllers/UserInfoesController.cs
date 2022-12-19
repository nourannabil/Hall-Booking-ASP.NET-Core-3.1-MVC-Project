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
    public class UserInfoesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserInfoesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: UserInfoes
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");
            return View(await _context.UserInfos.ToListAsync());
        }

        // GET: UserInfoes/Details/5
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
            var userInfo = await _context.UserInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fname,Lname,PhoneNumber,Email,Address,Gender,DateOfBirth,ImagePath,ImageFile")] UserInfo userInfo)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            if (ModelState.IsValid)
            {
                if (userInfo.ImageFile != null)
                {
                    string wwwrootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + userInfo.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await userInfo.ImageFile.CopyToAsync(filestream);
                    }
                    userInfo.ImagePath = fileName;
                    _context.Add(userInfo);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Edit/5
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

            var userInfo = await _context.UserInfos.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Fname,Lname,PhoneNumber,Email,Address,Gender,DateOfBirth,ImagePath,ImageFile")] UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return NotFound();
            }
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            if (ModelState.IsValid)
            {
                try
                {
                    if (userInfo.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + userInfo.ImageFile.FileName;
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await userInfo.ImageFile.CopyToAsync(filestream);
                        }
                        userInfo.ImagePath = fileName;
                    }
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.Id))
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
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
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

            var userInfo = await _context.UserInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var userInfo = await _context.UserInfos.FindAsync(id);
            _context.UserInfos.Remove(userInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(decimal id)
        {
            return _context.UserInfos.Any(e => e.Id == id);
        }
    }
}
