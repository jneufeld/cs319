using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DREAM.Models
{
    public enum Gender
    {
        Unknown = 0,
        Male,
        Female,
    }

    public class Patient
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string AgencyID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Gender { get; set; }

        public int Age { get; set; }
    }
}