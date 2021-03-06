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
using DREAM.CustomMembership;

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
            return View(new LoginModel());
        }

        //
        // POST: /Account/Login
        /// <summary>
        /// Logs in a specified user
        /// </summary>
        /// <param name="model">The model containing all necessary login information</param>
        /// <param name="returnUrl">The returnUrl </param>
        /// <returns>Redirects the user to the home page if login was successful, else the change password
        /// page if the user's current password in older than 42 days</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {
                using (DREAMContext db = new DREAMContext())
                {
                    MembershipUser user = Membership.GetUser(model.UserName);

                    if (user.LastPasswordChangedDate < DateTime.Now.AddDays(-42))
                    {
                        RouteValueDictionary routes = new RouteValueDictionary();
                        routes.Add("userName", model.UserName);
                        routes.Add("success", true);
                        routes.Add("statusMessage", "Your Password is greater than 42 days old. Please change your password to continue using the DREAM system.");
                        return RedirectToAction("ChangePassword", "Users", routes);
                    }

                }
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

        /// <summary>
        /// Directs the specified user to the change password page
        /// </summary>
        /// <param name="userName">The user name of the user to change their passwrod</param>
        /// <param name="success">A boolean to indicate a password has been changed successfully</param>
        /// <returns>A view for a user to change their passwrod</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword(String userName, String statusMessage, bool success = false)
        {
            ChangePasswordModel pm = new ChangePasswordModel();
            pm.UserName = userName;
            ViewBag.success = success;
            ViewBag.StatusMessage = statusMessage;
            return View(pm);
        }

        /// <summary>
        /// Changes the specified user's password
        /// </summary>
        /// <param name="model">The model containing the required user's information</param>
        /// <returns>Redirects the user to the home page if there is no user, else to the PasswordChangeConfirm 
        /// page to confirm the user has changed their password</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user = Membership.GetUser(model.UserName);
                if (user != null)
                {
                    bool success = ViewBag.success = false;
                    RouteValueDictionary routes = new RouteValueDictionary();
                    routes.Add("user", model.UserName);

                    try
                    {
                        success = user.ChangePassword(model.CurrentPassword, model.NewPassword);
                    }
                    catch (PreviouslyUsedPasswordException)
                    {
                        routes.Add("statusMessage", "Your Password was not changed, please enter a new password that does not match any of the previous passwords you have used for the last 252 days.");
                    }
                    catch (ArgumentException)
                    {
                        if (!routes.ContainsKey("statusMessage"))
                        {
                            routes.Add("statusMessage", "Your Password was not changed.  Please check that the current password you have entered is correct, and enter a new password that contains a capital letter, a number, a special character, and is at least 8 characters in length.");
                        }
                    }

                    if (success && !routes.ContainsKey("statusMessage"))
                    {
                        routes.Add("statusMessage", "Your Password has been successfully changed.");
                    }
                    else if (!routes.ContainsKey("statusMessage"))
                    {
                        routes.Add("statusMessage", "Your Password was not changed, please check that the current password you have entered is correct.");
                    }
                    if (Request.IsAuthenticated)
                        return RedirectToAction("PasswordChangeConfirm", "Users", routes);
                    else
                    {
                        RouteValueDictionary routs = new RouteValueDictionary();
                        routs.Add("returnUrl", null);
                        return RedirectToAction("Login", "Users", routs);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(model.UserName, "ModelState is not valid");
                return View(model);
            }
        }

        /// <summary>
        /// Directs the specified user to the password confirmation page
        /// </summary>
        /// <param name="user">The user who's password was changed</param>
        /// <param name="statusMessage">The message to let the user know if their password change was successful</param>
        /// <returns>A page showing the confirmation of a user's password change</returns>
        [HttpGet]
        public ActionResult PasswordChangeConfirm(String user, String statusMessage)
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

        /// <summary> Resets the user password </summary>
        /// <param name="email"> The email of the user who wants to reset his/her password </param>
        /// <param name="statusMessage"> Status message for any errors </param>
        /// <param name="success"> A true/false variable indicating whether or not a password has been successfully reset yet or not </param>
        /// <returns> The main Keyword Admin page on success and stays on the same page upon error </returns>
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

        /// <summary> The actual logic that implements resetting a password.  Note that the user does not have to be logged in to make this request. </summary>
        /// <param name="passwordResetRequestModel"> The password reset request model object containing the email of the user who wants to reset his/her password </param>>
        /// <returns> The main project page upon success; the same page with an error message upon failure </returns>
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
                        return RedirectToAction("Index", "Home", rVDictionary);
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
                ModelState.AddModelError("", "ModelState is not valid");
                return View(passwordResetRequestModel);
            }
        }

    }

}
