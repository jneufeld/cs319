using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public enum ReferenceType
    {
        URL = 0,
        FILE = 1,
        TEXT = 2,
        REQUEST = 3,
    }

    public class Reference
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public string Value { get; set; }
        public int QuestionID { get; set; }
    }
}