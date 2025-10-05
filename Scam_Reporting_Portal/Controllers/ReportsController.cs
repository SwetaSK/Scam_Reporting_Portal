using Scam_Reporting_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scam_Reporting_Portal.Controllers
{
    public class ReportsController : Controller
    {
        private ScamPortalContext db = new ScamPortalContext();

        // GET: /Reports
        // For Admin: show all reports. For User: show only their reports.
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Account");

            if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
            {
                // Admin sees all reports
                var all = db.ScamReports
                            .OrderByDescending(r => r.CreatedAt)
                            .ToList();
                return View(all);
            }
            else
            {
                // Normal user sees only own reports
                int userId = (int)Session["UserId"];
                var mine = db.ScamReports
                           .Where(r => r.UserId == userId)
                           .OrderByDescending(r => r.CreatedAt)
                           .ToList();
                return View(mine);
            }
        }
        // GET: /Reports/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: /Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScamReport model)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            model.UserId = (int)Session["UserId"];
            model.CreatedAt = DateTime.UtcNow;
            model.Status = (int)ReportStatus.Pending;

            db.ScamReports.Add(model);
            db.SaveChanges();
            TempData["Message"] = "Report submitted successfully.";
            return RedirectToAction("Index");
        }

        // GET: /Reports/Details/5
        public ActionResult Details(int id)
        {
            var report = db.ScamReports.Find(id);
            if (report == null) return HttpNotFound();

            // only owner or admin can view
            if (Session["Role"]?.ToString() != "Admin" && (int)Session["UserId"] != report.UserId)
                return RedirectToAction("Login", "Account");

            return View(report);
        }
        
    }
}