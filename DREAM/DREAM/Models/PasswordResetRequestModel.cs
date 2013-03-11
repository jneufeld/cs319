using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class PasswordResetRequestModel
    {
        [Required]
        public string Email { get; set; }
    }
}