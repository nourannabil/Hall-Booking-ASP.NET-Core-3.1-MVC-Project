using First_Project2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace First_Project2.Controllers
{
    public class LoginAndRegistrationController : Controller
    { 
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnviroment;

        public LoginAndRegistrationController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            this.webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fname,Lname,PhoneNumber,Email")] UserInfo userInfo, string username, string password)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInfo);
                await _context.SaveChangesAsync();

                var LastId = _context.UserInfos.OrderByDescending(p => p.Id).FirstOrDefault().Id;

                Login login1 = new Login();
                login1.RoleId = 2;
                login1.UserName = username;
                login1.Password = password;
                login1.UserId = LastId;
                _context.Add(login1);
                await _context.SaveChangesAsync();
                return RedirectToAction("LoginIn");
            }
            return View(userInfo);
        }


        public IActionResult LoginIn()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginIn([Bind("UserName,Password")] Login login)
        {
            var auth = _context.Logins.Where(x => x.UserName == login.UserName &&
                                             x.Password == login.Password ).SingleOrDefault();

            if (auth != null)
            {
                var userInfo = _context.UserInfos.Where(x => x.Id == auth.UserId).FirstOrDefault();
                
                switch (auth.RoleId)
                {

                    //for Admin
                    case 1:
                        HttpContext.Session.SetInt32("RoleId", (int)auth.RoleId);
                        HttpContext.Session.SetInt32("AdminId", (int)auth.UserId);
                        HttpContext.Session.SetString("UserName", auth.UserName);

                        if(userInfo.ImagePath != null)
                        {
                            HttpContext.Session.SetString("UserImage", userInfo.ImagePath);
                        }
                            HttpContext.Session.SetString("FirstName", userInfo.Fname);
                            HttpContext.Session.SetString("LastName", userInfo.Lname);
                            HttpContext.Session.SetString("UserEmail", userInfo.Email);

                        var claims1 = new List<Claim>
                        {
                         new Claim(ClaimTypes.NameIdentifier, Convert.ToString(auth.UserId))
                        };

                        var identity1 = new ClaimsIdentity(claims1, "canlog");

                        ClaimsPrincipal claimsPrincipal1 = new ClaimsPrincipal(identity1);

                        await HttpContext.SignInAsync("canlog", claimsPrincipal1);

                        return RedirectToAction("Admin", "DashBoard");

                    //for User
                    case 2:
                        HttpContext.Session.SetInt32("RoleId", (int)auth.RoleId);
                        HttpContext.Session.SetInt32("ClientId", (int)auth.UserId);
                        HttpContext.Session.SetString("UserName", auth.UserName);

                        if (userInfo.ImagePath != null)
                        {
                            HttpContext.Session.SetString("UserImage", userInfo.ImagePath);
                        }
                            HttpContext.Session.SetString("FirstName", userInfo.Fname);
                            HttpContext.Session.SetString("LastName", userInfo.Lname);
                            HttpContext.Session.SetString("UserEmail", userInfo.Email);


                        var claims2 = new List<Claim>
                        {
                         new Claim(ClaimTypes.NameIdentifier, Convert.ToString(auth.UserId))
                        };

                        var identity2 = new ClaimsIdentity(claims2, "canlog");

                        ClaimsPrincipal claimsPrincipal2 = new ClaimsPrincipal(identity2);

                        await HttpContext.SignInAsync("canlog", claimsPrincipal2);

                        return RedirectToAction("Users", "DashBoard");
                }
            }
            else
            {
                ViewData["Message"] = "Invalid username / password";
                return View();
            }

                return View();

        }

        public async Task <IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("LoginIn", "LoginAndRegistration");
        }

    }
}
