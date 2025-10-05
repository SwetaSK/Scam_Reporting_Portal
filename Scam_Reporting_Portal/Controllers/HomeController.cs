using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scam_Reporting_Portal.Models;

namespace Scam_Reporting_Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Username = Session["Username"];
            return View();
        }
    }
}