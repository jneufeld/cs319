using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public class UserGroup
    {
        public int ID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public int GroupID { get; set; }

        /*
        public virtual MembershipUser User
        {
            get
            {
                return Membership.GetUser(UserID);
            }
        }
        */
    }
}