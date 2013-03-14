using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using DREAM.Models;
using System.Web.Routing;

namespace DREAM.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(model.UserName, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        //[AllowAnonymous] - allow only logged in user
        public ActionResult ChangePassword(bool success=false){
            ViewBag.success = success;
            return View();
        }

        [HttpPost]
        //[AllowAnonymous] - allow only logged in user
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model){
            if(ModelState.IsValid) {
                MembershipUser user = Membership.GetUser(model.UserName);
                if (user != null && model.NewPassword == model.NewPassword2)
                {
                    user.ChangePassword(model.CurrentPassword, model.NewPassword);
                    // redirect to the GET page, so if a user refreshes the page they won’t try to change their password again
                    RouteValueDictionary routes = new RouteValueDictionary();
                    routes.Add("success", true);
                    return RedirectToAction("ChangePassword", "Users", routes);
                }
                else return View(model); //add error message saying user can't be null or new passwords must match...;
            }
            else {
                ModelState.AddModelError(model.UserName, "ModelState is not valid");
                return View(model);
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}
