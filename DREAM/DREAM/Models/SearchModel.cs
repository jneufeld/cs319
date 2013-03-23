using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class SearchViewModel
    {
        [Required]
        [Display (Name = "Search Query")]
        public String Query { get; set; }

        public IList<RequestViewModel> Results { get; set; }

        public bool Executed { get; set; }

        public SearchViewModel()
        {
            Query = "";
            Executed = false;
            Results = new List<RequestViewModel>();
        }
    }
}