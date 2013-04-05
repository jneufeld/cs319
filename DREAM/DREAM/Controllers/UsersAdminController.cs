using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DREAM.Controllers
{
    [Authorize(Roles=Role.ADMIN)]
    public class UsersAdminController : Controller
    {
        private DREAMContext db = new DREAMContext();

        public ActionResult Index()
        {
            MembershipUserCollection users = Membership.GetAllUsers();
            ViewBag.UsersAdminActive = true;
            return View(users);
        }

        [HttpGet]
        public ActionResult Register()
        {
            populateAssignedGroups();
            populateAssignedRoles();
            ViewBag.UsersAdminActive = true;
            return View(new RegisterUserModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel model, int[] selectedGroups, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    try
                    {
                        MembershipUser user = Membership.CreateUser(
                            model.UserName,
                            model.Password,
                            model.Email
                            );
                        user.IsApproved = model.Enabled;
                        Membership.UpdateUser(user);

                        UserProfile profile = new UserProfile
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            UserId = (Guid)user.ProviderUserKey,
                        };
                        db.UserProfiles.Add(profile);

                        if (user != null)
                        {
                            updateRoles(user, selectedRoles);
                            updateGroups(user, selectedGroups);

                            trans.Complete();
                            return RedirectToAction("Edit", "UsersAdmin", new { username = user.UserName });
                        }
                        ModelState.AddModelError("", "Failed to create new user.");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e.Message);
                    }
                }
            }

            populateAssignedGroups(selectedGroups:selectedGroups);
            populateAssignedRoles(selectedRoles:selectedRoles);
            ViewBag.UsersAdminActive = true;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string username)
        {
            MembershipUser user = Membership.GetUser(username);
            UserModel model = new UserModel(user);

            populateAssignedGroups(user);
            populateAssignedRoles(user);
            ViewBag.UsersAdminActive = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserModel model, int[] selectedGroups, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user = Membership.GetUser(model.UserName);

                using (TransactionScope trans = new TransactionScope())
                {
                    try
                    {
                        user.Email = model.Email;
                        user.IsApproved = model.Enabled;
                        Membership.UpdateUser(user);

                        UserProfile profile = UserProfile.GetFor(user);
                        profile.FirstName = model.FirstName;
                        profile.LastName = model.LastName;
                        db.Entry(profile).State = EntityState.Modified;

                        updateRoles(user, selectedRoles);
                        updateGroups(user, selectedGroups);

                        trans.Complete();
                        return RedirectToAction("Edit", new { username = user.UserName });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            populateAssignedGroups(selectedGroups:selectedGroups);
            populateAssignedRoles(selectedRoles:selectedRoles);
            ViewBag.UsersAdminActive = true;
            return View(model);
        }

        private void populateAssignedGroups(MembershipUser user=null, int[] selectedGroups=null)
        {
            selectedGroups = selectedGroups ?? new int[] { };
            HashSet<int> userGroups = new HashSet<int>();
            List<AssignedGroupModel> viewModel = new List<AssignedGroupModel>();

            if(user != null)
            {
                userGroups.UnionWith(
                    db.UserGroups.Where(ug => ug.UserID == (Guid)user.ProviderUserKey).Select(ug => ug.GroupID));
            }

            foreach(Group group in db.Groups)
            {
                viewModel.Add(new AssignedGroupModel
                {
                    GroupID = group.ID,
                    GroupName = group.Name,
                    Assigned = userGroups.Contains(group.ID) || selectedGroups.Contains(group.ID),
                });
            }

            ViewBag.Groups = viewModel;
        }

        private void populateAssignedRoles(MembershipUser user=null, string[] selectedRoles=null)
        {
            selectedRoles = selectedRoles ?? new string[] { };
            HashSet<string> userRoles = new HashSet<string>();
            List<AssignedRoleModel> viewModel = new List<AssignedRoleModel>();

            if(user != null)
            {
                userRoles.UnionWith(Roles.GetRolesForUser(user.UserName));
            }

            foreach(string role in Roles.GetAllRoles())
            {
                viewModel.Add(new AssignedRoleModel
                {
                    RoleName = role,
                    Assigned = userRoles.Contains(role) || selectedRoles.Contains(role),
                });
            }

            ViewBag.Roles = viewModel;
        }

        private void updateRoles(MembershipUser user, string[] selectedRoles)
        {
            string[] currentRoles = Roles.GetRolesForUser(user.UserName);
            string[] rolesToDelete = currentRoles.Except(selectedRoles).ToArray();
            string[] rolesToAdd = selectedRoles.Intersect(Roles.GetAllRoles()).Except(currentRoles).ToArray();

            if (rolesToDelete.Length > 0)
            {
                Roles.RemoveUserFromRoles(user.UserName, rolesToDelete);
            }
            if (rolesToAdd.Length > 0)
            {
                Roles.AddUserToRoles(user.UserName, rolesToAdd);
            }
        }

        private void updateGroups(MembershipUser user, int[] selectedGroups)
        {
            List<UserGroup> toDelete = db.UserGroups.Where(ug => ug.UserID.Equals((Guid)user.ProviderUserKey) &&
                !selectedGroups.Contains(ug.GroupID)).ToList();

            foreach (UserGroup userGroup in toDelete)
            {
                db.UserGroups.Remove(userGroup);
            }

            List<int> currentGroupIds = db.UserGroups.Where(ug => ug.UserID.Equals((Guid)user.ProviderUserKey)).Select(ug => ug.GroupID).ToList();
            foreach (int groupId in selectedGroups.Except(currentGroupIds))
            {
                db.UserGroups.Add(new UserGroup
                {
                    GroupID = groupId,
                    UserID = (Guid)user.ProviderUserKey,
                });
            }
            db.SaveChanges();
        }
    }
}
