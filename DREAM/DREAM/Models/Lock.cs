using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class Lock
    {
        public int ID;
        [Required]
        public DateTime ExpireTime { get; set; }
	    [Required]
        public Guid UserID { get; set; }
        [Required]
        public int RequestID { get; set; }
    }
}