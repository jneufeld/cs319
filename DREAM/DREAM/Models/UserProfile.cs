using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace DREAM.Models
{
    public class UserProfile
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static UserProfile GetFor(MembershipUser user)
        {
            using (DREAMContext db = new DREAMContext())
            {
                return db.UserProfiles.Find((Guid)user.ProviderUserKey);
            }
        }
    }
}
