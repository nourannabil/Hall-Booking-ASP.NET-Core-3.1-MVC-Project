using First_Project2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace First_Project2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DashboardController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;

        }
        

        public IActionResult Admin()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.AdminUserName = HttpContext.Session.GetString("UserName");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);
            var modelContext2 = _context.UserInfos.ToList();
            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);
            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");
            ViewBag.TotalUserEnter = modelContext2.Count();
            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);
            
            var statics = _context.Statistics.FirstOrDefault();
            ViewBag.Static = statics;
            

            return View();
        }
        
        public IActionResult Users()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientUserName = HttpContext.Session.GetString("UserName");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.ClientFName = HttpContext.Session.GetString("FirstName");
            ViewBag.ClientLName = HttpContext.Session.GetString("LastName");
            
            var hallCollection = _context.Halls.ToList().Take(3);

            return View(hallCollection);
        }

        public IActionResult MyProfile(decimal? id)
        {
         
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.ClientFName = HttpContext.Session.GetString("FirstName");
            ViewBag.ClientLName = HttpContext.Session.GetString("LastName");

            var userInfo =  _context.UserInfos
               .FirstOrDefault(m => m.Id == id);

            var log = _context.Logins.FirstOrDefault(x => x.UserId == id);
            ViewBag.log = log;

            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        public async Task<IActionResult> EditProfile(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.ClientFName = HttpContext.Session.GetString("FirstName");
            ViewBag.ClientLName = HttpContext.Session.GetString("LastName");

            var userInfo = await _context.UserInfos.FindAsync(id);

            var log = _context.Logins.FirstOrDefault(x => x.UserId == id);
            ViewBag.log = log;

            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Id,Fname,Lname,PhoneNumber,Email,Address,Gender,DateOfBirth,ImagePath,ImageFile")] UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return NotFound();
            }

            var log = _context.Logins.FirstOrDefault(x => x.UserId == id);
            ViewBag.log = log;

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
                return RedirectToAction("MyProfile" , new { Id = id });
            }
            return View(userInfo);
        }

        private bool UserInfoExists(decimal id)
        {
            throw new NotImplementedException();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> MyBooking (decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.ClientUserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.ClientFName = HttpContext.Session.GetString("FirstName");
            ViewBag.ClientLName = HttpContext.Session.GetString("LastName");

            var userInfo = _context.UserInfos
               .FirstOrDefault(m => m.Id == id);

            var BookDetails = from item in _context.HallBookings where item.UserId == userInfo.Id select item;
            ViewBag.Booking = BookDetails;

            var List = await (_context.Requests
                .Include(r => r.Booking)
                .Include(r => r.Category)
                .Include(r => r.Hall)
                .Include(r => r.Pay)
                .Include(r => r.User)
                .Where(x => x.UserId == userInfo.Id)).ToListAsync();

            return View(List);
        }

        ////////////Bill/////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> Bill(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId");
            ViewBag.ClientUserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.ClientFName = HttpContext.Session.GetString("FirstName");
            ViewBag.ClientLName = HttpContext.Session.GetString("LastName");

            var payment = _context.Payments.FirstOrDefault(m => m.Id == id);

            var List = await _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.Card)
                .Include(p => p.Hall)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(List);
        }


        //////////////Report///////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult Report()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var UserInfo = _context.UserInfos.ToList();
            var Hall = _context.Halls.ToList();
            var CategoryHall = _context.CategoryHalls.ToList();
            var HallBooking = _context.HallBookings.ToList();
            var Request = _context.Requests.ToList();

            //Join
            var multiTable = from cat in CategoryHall
                             join hall in Hall on cat.Id equals hall.CategoryId
                             join book in HallBooking on hall.Id equals book.HallId
                             join req in Request on book.Id equals req.BookingId
                             join user in UserInfo on req.UserId equals user.Id
                             select new JoinTable { categoryHall = cat, hall = hall, hallBooking = book, request = req , userInfo = user };

            var modelContext = _context.HallBookings.Include(h => h.Category).Include(h => h.Hall).Include(h => h.User);

            var modelContext5 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User).Where(x => x.Status != "Rejected" && x.Status != "Pending");

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            var modelContext4 = _context.Halls.Include(h => h.Category);

            var allBooking = modelContext5.Where(x => x.Status != "Rejected").Count(x => x.Hall.Id == x.HallId);

            var allHall = modelContext4.Count();


            ViewBag.TotbookedHall = modelContext5.Count(x => x.Status == "Approved");
            ViewBag.TotPendingbooked = modelContext5.Count(x => x.Status == "Pending");
            ViewBag.TotRejectedBooked = modelContext5.Count(x => x.Status == "Rejected");

            ViewBag.TotUnbookedHall = allHall - allBooking;

            ViewBag.TotHall = allHall;

            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

            ViewBag.TotalUserEnter = modelContext2.Count();

            var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<Request>>(multiTable, modelContext1);
            
            return View(model3);
        }


        [HttpPost]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate , DateTime? year)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var UserInfo = _context.UserInfos.ToList();
            var Hall = _context.Halls.ToList();
            var CategoryHall = _context.CategoryHalls.ToList();
            var HallBooking = _context.HallBookings.ToList();
            var Request = _context.Requests.ToList();

            int? Targetyear = null;

            if (year != null)
            {
                Targetyear = year.Value.Year;
            }


            var multiTable = from cat in CategoryHall
                             join hall in Hall on cat.Id equals hall.CategoryId
                             join book in HallBooking on hall.Id equals book.HallId
                             join req in Request on book.Id equals req.BookingId
                             join user in UserInfo on req.UserId equals user.Id
                             select new JoinTable { categoryHall = cat, hall = hall, hallBooking = book, request = req, userInfo = user };

            var modelContext = _context.HallBookings.Include(h => h.Category).Include(h => h.Hall).Include(h => h.User);

            var modelContext5 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User).Where(x => x.Status != "Rejected" && x.Status != "Pending");

            var modelContext2 = _context.UserInfos.ToList();

            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);

            var modelContext4 = _context.Halls.Include(h => h.Category);

            var allBooking = modelContext5.Where(x => x.Status != "Rejected").Count(x => x.Hall.Id == x.HallId);

            var allHall = modelContext4.Count();


            if (startDate == null && endDate == null && year == null)
            {

                ViewBag.TotbookedHall = modelContext5.Count(x => x.Status == "Approved");
                ViewBag.TotPendingbooked = modelContext5.Count(x => x.Status == "Pending");
                ViewBag.TotRejectedBooked = modelContext5.Count(x => x.Status == "Rejected");

                ViewBag.TotUnbookedHall = allHall - allBooking;

                ViewBag.TotHall = allHall;

                ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

                ViewBag.TotalUserEnter = modelContext2.Count();

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<Request>>(multiTable, await modelContext1.ToListAsync());

                return View(model3);
            }
            else if (startDate == null && endDate != null && year == null)
            {
                ViewBag.TotbookedHall = modelContext5.Count(x => x.Status == "Approved");
                ViewBag.TotPendingbooked = modelContext5.Count(x => x.Status == "Pending");
                ViewBag.TotRejectedBooked = modelContext5.Count(x => x.Status == "Rejected");

                ViewBag.TotUnbookedHall = allHall - allBooking;

                ViewBag.TotHall = allHall;

                ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

                ViewBag.TotalUserEnter = modelContext2.Count();

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<Request>>(multiTable, await modelContext1.Where(x => x.Booking.BookingDate.Value.Date == endDate).ToListAsync());

                return View(model3);
            }
            else if (startDate != null && endDate == null && year == null)
            {
                ViewBag.TotbookedHall = modelContext5.Count(x => x.Status == "Approved");
                ViewBag.TotPendingbooked = modelContext5.Count(x => x.Status == "Pending");
                ViewBag.TotRejectedBooked = modelContext5.Count(x => x.Status == "Rejected");

                ViewBag.TotUnbookedHall = allHall - allBooking;

                ViewBag.TotHall = allHall;

                ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

                ViewBag.TotalUserEnter = modelContext2.Count();

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<Request>>(multiTable, await modelContext1.Where(x => x.Booking.BookingDate.Value.Date == startDate).ToListAsync());

                return View(model3);
            }
            else if (startDate == null && endDate == null && year != null)
            {
                ViewBag.TotbookedHall = modelContext5.Count(x => x.Status == "Approved");
                ViewBag.TotPendingbooked = modelContext5.Count(x => x.Status == "Pending");
                ViewBag.TotRejectedBooked = modelContext5.Count(x => x.Status == "Rejected");

                ViewBag.TotUnbookedHall = allHall - allBooking;

                ViewBag.TotHall = allHall;

                ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

                ViewBag.TotalUserEnter = modelContext2.Count();

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<Request>>(multiTable, await modelContext1.Where(x => x.Booking.BookingDate.Value.Year == Targetyear).ToListAsync());

                return View(model3);
            }
            else
            {

                ViewBag.TotbookedHall = modelContext5.Count(x => x.Status == "Approved");
                ViewBag.TotPendingbooked = modelContext5.Count(x => x.Status == "Pending");
                ViewBag.TotRejectedBooked = modelContext5.Count(x => x.Status == "Rejected");

                ViewBag.TotUnbookedHall = allHall - allBooking;

                ViewBag.TotHall = allHall;

                ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);

                ViewBag.TotalUserEnter = modelContext2.Count();

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<Request>>(multiTable, await modelContext1.Where(x => x.Booking.BookingDate >= startDate && x.Booking.BookingDate <= endDate).ToListAsync());

                return View(model3);
            }

        }

        ///Charts////////////////////////////////////////////////////////////////////////////

        public IActionResult Chart()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            var modelContext1 = _context.Requests.Include(r => r.Booking).Include(r => r.Category).Include(r => r.Hall).Include(r => r.Pay).Include(r => r.User);
            var modelContext2 = _context.UserInfos.ToList();
            var modelContext3 = _context.Payments.Include(p => p.Booking).Include(p => p.Card).Include(p => p.Hall).Include(p => p.User);
            ViewBag.TotbookedHall = modelContext1.Count(x => x.Status == "Approved");
            ViewBag.TotalUserEnter = modelContext2.Count();
            ViewBag.TotalRevenue = modelContext3.Sum(x => x.Hall.Price);
            var statics = _context.Statistics.FirstOrDefault();
            ViewBag.Static = statics;

            return View();
        }

        //The Price for each Hall
        public List<object> GetChart()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            List<object> data = new List<object>();

            List<string> HallName = _context.Halls.Select(x => x.Name).ToList();
            data.Add(HallName);

            List<decimal> HallPrice = _context.Halls.Select(x => x.Price).ToList();
            data.Add(HallPrice);

            return data;
        }


        // How many Times the Hall Booked
        public List<object> GetChart2()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            List<object> data1 = new List<object>();

            List<string> modelContext1 = _context.HallBookings.Select(x => x.Hall.Name).Distinct().ToList();
            data1.Add(modelContext1);


            List<int> sum = new List<int>();

            foreach(var item in modelContext1)
            {
                sum.Add(_context.HallBookings.Count(x => x.Hall.Name == item));
            }

            data1.Add(sum);

            return data1;
        }

        // The Number of Approved & UnApproved & Appended booing Order 
        public List<object> GetChart3()
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");

            List<object> data2 = new List<object>();

            List<string> modelContext1 = _context.Requests.Select(x => x.Status).Distinct().ToList();
            data2.Add(modelContext1);


            List<int> sum = new List<int>();

            foreach (var item in modelContext1)
            {
                sum.Add(_context.Requests.Count(x => x.Status == item));
            }

            data2.Add(sum);

            return data2;
        }
    }
}


      


