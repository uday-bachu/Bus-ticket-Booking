using Busticket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Busticket.Controllers
{
    public class HomeController : Controller
    {
        DbBooking context = new DbBooking();


        public ActionResult About()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection frmc)
        {
            string Id = frmc["LoginId"];
            string password = frmc["Password"];
            string role = frmc["Role"];

            int loginId = int.Parse(Id);
            Session.Add("Id", loginId);

            Login login = context.Logins
                .Where(l => l.LoginId == loginId && l.password == password && l.Role == role).FirstOrDefault();
            if (login == null)
            {
                return View("Error");
            }
            if (login.Role == "Admin") {
                return RedirectToAction("AdminIndex", "Admin");
            }
            else if (login.Role =="Vendor")
            {
                return RedirectToAction("VendorIndex", "Vendor");
            }
            else if(login.Role == "Passenger")
            {
                return RedirectToAction("Details", "Passenger");
            }
            else
            {
                ViewBag.Error = "Please enter the LoginId/Password Correctly";
                return View("Error");
            }

           
        }

        
    }      
}
