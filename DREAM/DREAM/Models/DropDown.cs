﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class DropDown
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string FullName { get; set; }
        public bool Enabled { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}