using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            using (DBUserSignupLoginEntities dBUserSignupLoginEntities = new DBUserSignupLoginEntities())
            {
                TBUserInfo user = dBUserSignupLoginEntities.TBUserInfoes.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Create(TBUserInfo user)
        {
            try
            {
                using (DBUserSignupLoginEntities dBUserSignupLoginEntities= new DBUserSignupLoginEntities())
                {
                    dBUserSignupLoginEntities.TBUserInfoes.Add(user);
                    dBUserSignupLoginEntities.SaveChanges();
                }

                return RedirectToAction("User", "Home");
            }
            catch
            {
                return View();
            }

        }


        [HttpPost]
        public ActionResult EditUser(TBUserInfo user)
        {
            try
            {
                using (DBUserSignupLoginEntities dBUserSignupLoginEntities = new DBUserSignupLoginEntities())
                {
                    // Attach the user object to the DbContext if it's not already attached
                    dBUserSignupLoginEntities.Entry(user).State = EntityState.Modified;

                    // Save changes
                    dBUserSignupLoginEntities.SaveChanges();
                }
                return RedirectToAction("User", "Home");
            }
            catch
            {
                // Error handling logic (e.g., display error message or log exception)
                return View(user); // Re-display the edit form with potential error messages
            }
        }


    }
}