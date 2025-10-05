using Scam_Reporting_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Scam_Reporting_Portal.Controllers
{
    public class AdminController : Controller
    {
        private ScamPortalContext db = new ScamPortalContext();
        private bool IsAdmin()
        {
            return Session["Role"] != null && Session["Role"].ToString() == "Admin";
        }


        public ActionResult Index()
        {
                        var reports = db.ScamReports
                            .Include("User")                 // include related User entity
                            .Where(r => r.Status == (int)ReportStatus.Pending)
                            .OrderBy(r => r.CreatedAt)
                            .ToList();

            return View(reports);
        }

        // POST: /Admin/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
           // if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var report = db.ScamReports.Find(id);
            if (report == null) return HttpNotFound();

            report.Status = (int)ReportStatus.Approved;
            report.ReviewedAt = DateTime.UtcNow;
            report.ReviewedBy = Session["UserName"]?.ToString();
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: /Admin/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(int id)
        {
            if (IsAdmin()) return RedirectToAction("Login", "Account");

            var report = db.ScamReports.Find(id);
            if (report == null) return HttpNotFound();

            report.Status = (int)ReportStatus.Rejected;
            report.ReviewedAt = DateTime.UtcNow;
            report.ReviewedBy = Session["UserName"]?.ToString();
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }

}