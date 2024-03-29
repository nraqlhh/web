using web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Validation;


namespace web.Controllers
{
    public class HomeController : Controller
    {
        DBUserSignupLoginEntities db = new DBUserSignupLoginEntities();

        private object checkLogin;


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public new ActionResult User()
        {
            return View(db.TBUserInfoes.ToList());
        }

        [HttpPost]
        public ActionResult Signup(TBUserInfo tBUserInfo)
        {
            if (ModelState.IsValid) // Ensures that the model state is valid
            {
                var existingUser = db.TBUserInfoes.FirstOrDefault(x => x.UsernameUs == tBUserInfo.UsernameUs);

                if (existingUser != null)
                {
                    ViewBag.Notification = "This account already exists.";
                    return View();
                }
                else
                {
                    var newuser = new TBUserInfo
                    {
                        UsernameUs = tBUserInfo.UsernameUs,
                        Email = tBUserInfo.Email
                    };

                    var passwordHasher = new PasswordHasher<TBUserInfo>();
                    newuser.PasswordUs = passwordHasher.HashPassword(newuser, tBUserInfo.PasswordUs);
                    newuser.RePasswordUs = newuser.RePasswordUs;

                    db.TBUserInfoes.Add(newuser);

                    try
                    {
                        db.SaveChanges();
                        ViewBag.RegistrationSuccess = true;
                        return RedirectToAction("Login", "Home");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }

                        // If there are validation errors, return the view with the model to display the error messages
                        return View(tBUserInfo);
                    }
                }
            }
            else
            {
                // Model state is not valid, return the view with the model to display validation errors
                return View(tBUserInfo);
            }
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBUserInfo tBLUserInfo)

        {
            var checkLogin = db.TBUserInfoes.Where(x => x.UsernameUs.Equals(tBLUserInfo.UsernameUs)).FirstOrDefault();
            if (checkLogin != null)
            {
                var enterpassword = tBLUserInfo.PasswordUs;
                var userpassword = checkLogin.PasswordUs;

                var passwordhasher = new PasswordHasher<string>();
                var passwordVerificationResult = passwordhasher.VerifyHashedPassword(null, userpassword, enterpassword).ToString();
                if (passwordVerificationResult == "Success")
                {

                    Session["IdusSS"] = tBLUserInfo.IdUs.ToString();
                    Session["UsernameSS"] = tBLUserInfo.UsernameUs.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Notification = "Wrong Username or password !!!";

                }

            }
            else
            {
                ViewBag.Notification = "Wrong Username or password !!!";

            }
            return View();
        }


        public ActionResult Details(int id)
        {
            // Fetch the specific user information from the database based on the ID
            var user = db.TBUserInfoes.Find(id);

            // Check if the user exists
            if (user == null)
            {
                return HttpNotFound(); // Handle case where user with specified ID is not found
            }

            // Return the user object to the Details view
            return View(user);
        }

    }
}
