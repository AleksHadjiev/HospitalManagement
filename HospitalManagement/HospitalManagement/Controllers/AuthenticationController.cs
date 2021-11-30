using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class AuthenticationController : BaseController
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            if (username != null || password != null)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Password = password
                };

                this._context.Users.Add(user);
                this._context.SaveChanges();

                return RedirectToAction("Login", "Authentication");
            }

            return RedirectToAction("Register", "Authentication");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username != null || password != null)
            {
                var user = this._context.Users
                    .FirstOrDefault(x => x.Username == username && x.Password == password);

                if (user != null)
                {
                    if (user.IsAdmin)
                    {
                        HttpContext.Response.Cookies.Append("adm", "true");
                    }

                    HttpContext.Response.Cookies.Append("id", user.Id.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Authentication");
        }
    }
}
