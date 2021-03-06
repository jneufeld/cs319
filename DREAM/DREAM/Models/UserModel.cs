﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name="User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public UserModel() { }

        public UserModel(MembershipUser user)
        {
            UserName = user.UserName;
            FirstName = "First Name";
            LastName = "Last Name";
            Email = user.Email;
            Enabled = user.IsApproved;

            UserProfile profile = UserProfile.GetFor(user);
            FirstName = profile.FirstName;
            LastName = profile.LastName;
        }
    }
}