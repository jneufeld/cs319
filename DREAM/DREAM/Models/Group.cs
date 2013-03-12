using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public class Group
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        /*
        public virtual List<MembershipUser> Users
        {
            get
            {
                return new List<MembershipUser>();
            }
        }
        */
    }
}