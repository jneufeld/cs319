using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class DropDown
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
    }
}