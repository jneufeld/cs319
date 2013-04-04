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
        Text,
        URL,
        File,
        Request,
    }

    public class Reference
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ReferenceType { get; set; }
        public string Value { get; set; }
        //public int QuestionID { get; set; }
    }
}