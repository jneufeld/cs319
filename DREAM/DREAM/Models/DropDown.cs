using System;
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
        public string Code { get; set; }
        public string FullName { get; set; }

        [NotMapped]
        public string StringID
        {
            get
            {
                return ID.ToString();
            }
            set
            {
                int tmp;
                Int32.TryParse(value, out tmp);
                // Check to see if parsing failed
                if (tmp != 0)
                    ID = tmp;
            }
        }
    }
}