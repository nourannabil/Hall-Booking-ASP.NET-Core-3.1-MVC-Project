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
    public class HallPhotoesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HallPhotoesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }


        // GET: HallPhotoes
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext = _context.HallPhotos
                .Include(h => h.Category)
                .Include(h => h.Hall);
            return View(await modelContext.ToListAsync());
        }

        // GET: HallPhotoes/Details/5
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

            var hallPhoto = await _context.HallPhotos
                .Include(h => h.Category)
                .Include(h => h.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hallPhoto == null)
            {
                return NotFound();
            }

            return View(hallPhoto);
        }

        /// <summary>
        /// For casscading DropDown List in create View
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


        // GET: HallPhotoes/Create
        public IActionResult Create()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name");
            return View();
        }


        // POST: HallPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagePath,ImageFile,CategoryId,HallId")] HallPhoto hallPhoto)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in hallPhoto.ImageFile)
                {
                    string stringFileName = Upload(item);
                    HallPhoto hallImage = new HallPhoto()
                    {
                        Id = hallPhoto.Id,
                        ImagePath = stringFileName,
                        CategoryId = hallPhoto.CategoryId,
                        HallId = hallPhoto.HallId,
                    };
                    _context.Add(hallImage);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("SeePhoto", new { Id = hallPhoto.HallId });
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", hallPhoto.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", hallPhoto.HallId);
            return View(hallPhoto);
        }

        // GET: HallPhotoes/Edit/5
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

            var hallPhoto = await _context.HallPhotos.FindAsync(id);
            if (hallPhoto == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "CategoryName", hallPhoto.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", hallPhoto.HallId);
            return View(hallPhoto);
        }

        // POST: HallPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImagePath,ImageFile,CategoryId,HallId")] HallPhoto hallPhoto)
        {
            if (id != hallPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hallPhoto.ImageFile != null)
                    {
                        foreach (var item in hallPhoto.ImageFile)
                        {
                            string stringFileName = Upload(item);
                            hallPhoto.ImagePath = stringFileName;
                            _context.Update(hallPhoto);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallPhotoExists(hallPhoto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("SeePhoto", new { Id = hallPhoto.HallId });
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryHalls, "Id", "Id", hallPhoto.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", hallPhoto.HallId);
            return View(hallPhoto);
        }


        private string Upload(IFormFile item)
        {
            string fileName = null;

            if (item != null)
            {
                string wwwrootPath = webHostEnvironment.WebRootPath;
                fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    item.CopyTo(filestream);
                }
            }
            return fileName;
        }


        // GET: HallPhotoes/Delete/5
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

            var hallPhoto = await _context.HallPhotos
                .Include(h => h.Category)
                .Include(h => h.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hallPhoto == null)
            {
                return NotFound();
            }

            return View(hallPhoto);
        }

        // POST: HallPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var hallPhoto = await _context.HallPhotos.FindAsync(id);
            _context.HallPhotos.Remove(hallPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction("SeePhoto", new { Id = hallPhoto.HallId });

        }

        private bool HallPhotoExists(decimal id)
        {
            return _context.HallPhotos.Any(e => e.Id == id);
        }

        ////////////////////////////////////////////////////////////////

        public IActionResult SeePhoto(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var Photo = from item in _context.HallPhotos
                       where (item.HallId == id)
                       select item;

            ViewBag.Photo = Photo;

            return View(Photo);
        }

    }
}
