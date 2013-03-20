using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class ReportModel
    {
        [Required]
        public string Name { get; set; }

        public IList<ChartModel> Charts { get; set; }
    }
}