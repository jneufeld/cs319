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
        public ActionResult ChangePassword(String userName, bool success=false){
            ChangePasswordModel pm = new ChangePasswordModel();
            pm.UserName = userName;
            ViewBag.success = success;
            return View(pm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model){
            if(ModelState.IsValid) {
                MembershipUser user = Membership.GetUser(model.UserName);
                if (user != null)
                {
                    bool success = ViewBag.success = user.ChangePassword(model.CurrentPassword, model.NewPassword);
                    RouteValueDictionary routes = new RouteValueDictionary();
                    routes.Add("user", model.UserName);
                    if (success)
                    {
                        routes.Add("statusMessage", "Your Password has been successfully changed");
                    }
                    else
                    {
                        routes.Add("statusMessage", "Your Password was not changed");
                    }
                    return RedirectToAction("Manage", "Users", routes);
                }
                else return RedirectToAction("Index", "Home");
            }
            else {
                ModelState.AddModelError(model.UserName, "ModelState is not valid");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Manage(String user, String statusMessage)
        {
            ViewBag.StatusMessage = statusMessage;
            return View();
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword(String email, String statusMessage, bool success = false)
        {
            PasswordResetRequestModel passwordResetRequestModel = new PasswordResetRequestModel();
            passwordResetRequestModel.Email = email;
            ViewBag.success = success;
            ViewBag.StatusMessage = statusMessage;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(PasswordResetRequestModel passwordResetRequestModel)
        {
            RouteValueDictionary rVDictionary = new RouteValueDictionary();

            if (ModelState.IsValid)
            {
                String username = Membership.GetUserNameByEmail(passwordResetRequestModel.Email);

                if (username != null)
                {
                    MembershipUser user = Membership.GetUser(username);

                    try
                    {
                        PasswordResetRequest.GenerateFor(user);
                        rVDictionary.Add("email", passwordResetRequestModel.Email);
                        rVDictionary.Add("success", true);
                        rVDictionary.Add("statusMessage", "Password Reset Successfully");
                        return RedirectToAction("ResetPassword", "Users", rVDictionary);
                    }
                    catch
                    {
                        ModelState.AddModelError(passwordResetRequestModel.Email, "Error in trying to reset password for user.");
                        rVDictionary.Add("email", passwordResetRequestModel.Email);
                        rVDictionary.Add("success", false);
                        rVDictionary.Add("statusMessage", "Error in trying to reset password for user.");
                        return View(passwordResetRequestModel);
                    }
                }
                else
                {
                    ModelState.AddModelError(passwordResetRequestModel.Email, "This user does not exist in the DREAM system.");
                    rVDictionary.Add("email", "");
                    rVDictionary.Add("success", false);
                    rVDictionary.Add("statusMessage", "There is no registered user in DREAM with that email.");
                    return RedirectToAction("ResetPassword", "Users", rVDictionary);
                }
            }
            else
            {
                //RouteValueDictionary rVDictionary = new RouteValueDictionary();
                //rVDictionary.Add("email", "");
                //rVDictionary.Add("success", true);
                //rVDictionary.Add("statusMessage", "There is no registered user in DREAM with that email.");
                return View(passwordResetRequestModel) ;
            }
        }
    }

}
