using Scam_Reporting_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Scam_Reporting_Portal.Controllers
{
    public class AccountController : Controller
    {
        ScamPortalContext db = new ScamPortalContext();

        // Register - GET
        public ActionResult Register()
        {
            return View();
        }
        // Register - POST
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid) return View(user);

            //check duplicate email
            if (db.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(user);
            }

            // Check for duplicate username
            if (db.Users.Any(u => u.Name == user.Name))
            {
                ModelState.AddModelError("Name", "Username already exists. Please try another name.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                /*user.Role = "User";*/ // default role
                db.Users.Add(user);
                db.SaveChanges();
                TempData["Message"] = "Registration successful. Please login!";
                return RedirectToAction("Login");
            }
            return View(user);
        }
        // Login - GET
        public ActionResult Login()
        {
            return View();
        }
        // Login - POST
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Enter email and password.";
                return View();
            }
            // check user in DB
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }


            if (user != null)
            {
                // Save to session
                Session["UserId"] = user.UserId;
                Session["UserName"] = user.Name;
                Session["UserRole"] = user.Role;

                if (user.Role == "Admin")
                {
                    // Redirect to Admin Controller → Index Action
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Reports"); // user -> Reports page
                }
            }
            else
            {
                ViewBag.Message = "Invalid username or password!";
                return View();
            }
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

       
    }
}