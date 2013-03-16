using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class SearchModel
    {
        [Required]
        [Display (Name = "Search Query")]
        public String query { get; set; }

        public IEnumerable<Request> results { get; set; }

        public bool executed { get; set; }

        public SearchModel()
        {
            executed = false;
            query = "Search exact patient first name";
        }
    }
}