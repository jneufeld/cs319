using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DREAM.Controllers
{
    public class UsersAdminController : Controller
    {
        public ActionResult Index()
        {
            MembershipUserCollection users = Membership.GetAllUsers();
            return View(users);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Edit(string username)
        {
        }
    }
}
