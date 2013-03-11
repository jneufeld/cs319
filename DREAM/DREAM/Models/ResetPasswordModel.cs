using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewPassword2 { get; set; }
    }
}