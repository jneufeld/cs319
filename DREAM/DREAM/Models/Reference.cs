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
        URL_VALUE = 0,
        FILE_VALUE = 1,
        TEXT_VALUE = 2,
        REQUEST_VALUE = 3,
    }

    public class Reference
    {
        public int id { get; set; }
        public ReferenceType referenceType { get; set; }
        public string value { get; set; }
        public int questionID { get; set; }
    }
}