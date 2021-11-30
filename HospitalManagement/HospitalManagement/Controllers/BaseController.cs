using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class BaseController : Controller
    {
        internal HospitalManagement_DbContext _context = new HospitalManagement_DbContext();

        internal bool IsAdminUser()
        {
            var userIdCookie = Request.Cookies["id"];
            Guid? userId = null;
            if (userIdCookie != null)
            {
                userId = Guid.Parse(userIdCookie);
            }

            if (userId != null)
            {
                var user = this._context.Users.FirstOrDefault(x => x.Id == userId);

                if (user.IsAdmin)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
