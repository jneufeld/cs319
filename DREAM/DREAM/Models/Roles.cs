using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public class Role
    {
        public const string VIEWER = "Viewer";
        public const string DI_SPECIALIST = "DI Specialist";
        public const string REPORTER = "Reporter";
        public const string ADMIN = "Admin";

        public static void CreateRoles()
        {
            foreach(string role in new string[] {VIEWER, DI_SPECIALIST, REPORTER, ADMIN})
            {
                if(!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }
        }
    }
}