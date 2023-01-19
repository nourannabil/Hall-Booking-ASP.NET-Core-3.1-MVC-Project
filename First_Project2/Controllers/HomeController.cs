using First_Project2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace First_Project2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Home()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            // category
            var category = from item in _context.CategoryHalls select item;
            ViewBag.CatInfo = category;

            //Home
            var HomePage = from item in _context.HomePages select item;
            ViewBag.HomePage = HomePage;

            // About
            var AboutPage = from item in _context.AboutPages select item;
            ViewBag.AboutPage = AboutPage;

            var modelContext4 = _context.Halls.Include(h => h.Category);
            ViewBag.TotalHall = modelContext4.Count();

            var modelContext2 = _context.UserInfos.ToList();
            ViewBag.TotalUserEnter = modelContext2.Count();

            // Contact us
            var ContactPage = _context.ContactUsPages.FirstOrDefault();
            ViewBag.ContactPage = ContactPage;

            // Testmoinal 
            var TestimonialPage = _context.TestimonialPages.ToList().Where(x => !x.Opinion.StartsWith("UnApproved"));
            ViewBag.TestimonialPage = TestimonialPage;

            return View();
        }

        public IActionResult About()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var AboutPage = from item in _context.AboutPages select item;
            ViewBag.AboutPage = AboutPage;

            var modelContext4 = _context.Halls.Include(h => h.Category);
            ViewBag.TotalHall = modelContext4.Count();

            var modelContext2 = _context.UserInfos.ToList();
            ViewBag.TotalUserEnter = modelContext2.Count();

            return View();
        }

        public IActionResult ContactUs()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var ContactPage = _context.ContactUsPages.FirstOrDefault();
            ViewBag.ContactPage = ContactPage;

            return View();
        }

       
        public IActionResult Contact()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Id,Lname,Fname,PhoneNumber,Email,Message,HomeId,UserId")] ContactUsPage contactUsPage)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");

            if (ModelState.IsValid)
            {
                _context.Add(contactUsPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Home));
            }

            return View();
        }

        public IActionResult Testimonial()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var TestimonialPage = _context.TestimonialPages.ToList().Where(x => !x.Opinion.StartsWith("UnApproved"));
            ViewBag.TestimonialPage = TestimonialPage;

            var test = _context.TestimonialPages.ToList();
            ViewBag.test = test;

            foreach (var item in TestimonialPage)
            {
                var img = _context.UserInfos.Where(x => x.Id == item.UserId).ToList();
                ViewBag.img = img;
            }

            return View(TestimonialPage);
        }
        
        ///////////////////////////////////////////////////////////////////////////////
        
        public IActionResult CatalogueHome()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            return View(_context.CategoryHalls);
        }

        public IActionResult HallMenuHome(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var hallCollection = from item in _context.Halls where item.CategoryId == id select item;

            var title = _context.CategoryHalls.SingleOrDefault(x => x.Id == id).CategoryName;
            ViewBag.desc = title;

            return View(hallCollection);
        }

        public IActionResult HallInformationHome(int id)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var allPhoto = from item in _context.HallPhotos where item.HallId == id select item;

            var allFeat = from item in _context.Features where item.HallId == id select item;

            var multiTable = Tuple.Create<IEnumerable<HallPhoto>, IEnumerable<Feature>>(allPhoto, allFeat);


            var hall = from item in _context.Halls
                       where (item.Id == id)
                       select item;
            ViewBag.HInfo = hall;

            var hally = _context.Halls.FirstOrDefault(x => x.Id == id);

            var category = from item in _context.CategoryHalls
                           where (item.Id == hally.CategoryId)
                           select item;
            ViewBag.CatInfo = category;

            return View(multiTable);
        }

        //////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.Name = "";
            ViewBag.Location = "";
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            var modelContext = _context.Halls.Include(h => h.Category);
            return View(modelContext.ToList());
        }

        [HttpPost]
        public IActionResult Search( string hallName , string location )
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            if (hallName == null && location == null)
            {
                ViewBag.Name = hallName;
                ViewBag.Location = location;
                var modelContext = _context.Halls.ToList();

                return View(modelContext);
            }
            else if (hallName != null && location == null)
            {
                ViewBag.Name = hallName;
                ViewBag.Location = location;
                var modelContext = _context.Halls.ToList().Where(x => x.Name.ToUpper().StartsWith(hallName.ToUpper()));
                return View(modelContext);

            } else if (hallName == null && location != null)
            {
                ViewBag.Name = hallName;
                ViewBag.Location = location.ToUpper();
                var modelContext = _context.Halls.ToList().Where(x => x.Location.ToUpper().StartsWith(location.ToUpper()));
                return View(modelContext);
            }else
            {
                ViewBag.Name = hallName.ToUpper();
                ViewBag.Location = location.ToUpper();
                var modelContext = _context.Halls.ToList().Where(x => x.Name.ToUpper().StartsWith(hallName.ToUpper()) && x.Location.ToUpper().StartsWith(location.ToUpper()));
                return View(modelContext);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
