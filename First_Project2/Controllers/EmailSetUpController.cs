using Microsoft.AspNetCore.Mvc;
using First_Project2.Models;
using System.Net;
using System.Net.Mail;
using System;
using System.Text.Unicode;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace First_Project2.Controllers
{
    public class EmailSetUpController : Controller
    {
        private readonly ModelContext _context;

        public EmailSetUpController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(First_Project2.Models.Email model)
        {

            try
            {

                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com");
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;

                NetworkCredential nc = new NetworkCredential("hallbookingteam12@outlook.com", "admin23-10-1999");
                smtp.EnableSsl = true;
                smtp.Credentials = nc;



                MailMessage mm = new MailMessage("hallbookingteam12@outlook.com", model.To);
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                mm.IsBodyHtml = true;

                smtp.Send(mm);
                ViewBag.Message = "Mail Has been Sent Successfully";

                return View();
            }
            catch (Exception)
            {
                throw;
            }

        }


        public bool SendEmail( string To , string Subject , string Body)
        {
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            ViewBag.UserImage = HttpContext.Session.GetString("UserImage");
            ViewBag.AdminFName = HttpContext.Session.GetString("FirstName");
            ViewBag.AdminLName = HttpContext.Session.GetString("LastName");

            try
            {

                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com");
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;

                NetworkCredential nc = new NetworkCredential("hallbookingteam12@outlook.com", "admin23-10-1999");
                smtp.EnableSsl = true;
                smtp.Credentials = nc;



                MailMessage mm = new MailMessage("hallbookingteam12@outlook.com", To , Subject , Body);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = System.Text.Encoding.UTF8;

                smtp.Send(mm);
                ViewBag.Message = "Mail Has been Sent Successfully";
                return true;
            }
            catch (Exception )
            {
                return false;   
            }

        }


        public JsonResult SendMailToUser(int id)
        {
            var request = _context.Requests.SingleOrDefault(x => x.Id == id);

            var user = _context.UserInfos.SingleOrDefault(x => x.Id == request.UserId); ;
            ViewBag.user = user;


            bool result = false;

            result = SendEmail(user.Email, "Booking Status",
                "<h1>Hello</h1>" + user.Fname + " " + user.Lname +
                "<h3>Thank you for booking and trust us </h3> " +
                "<h3> your booking request <strong>Approved</strong> </h3>" +
                "<p> you can compleate the process and pay for the booking </p>" +
                "<p> we wish you to enjoy your booking </p>");

            return new JsonResult(result);
        }
        public JsonResult SendRejectMailToUser(int id)
        {
            var request = _context.Requests.SingleOrDefault(x => x.Id == id);

            var user = _context.UserInfos.SingleOrDefault(x => x.Id == request.UserId); ;
            ViewBag.user = user;

            bool result = false;

            result = SendEmail(user.Email, "Booking Status",
                "<h1>Hello</h1>" + user.Fname + " " + user.Lname +
                "<h3> your booking request has been <strong>Rejected</strong> </h3>" +
                "<p> we are sorry you can try to book another hall </p>" +
                "<p> we wish you a happey day </p>");

            return Json(result);
        }
    }
}
