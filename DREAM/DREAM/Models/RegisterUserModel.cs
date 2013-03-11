using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class RegisterUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public RegisterUserModel()
        {
            Enabled = true;
        }
    }
}