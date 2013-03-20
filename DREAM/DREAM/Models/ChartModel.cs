using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class ChartModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public TimeRange TimeRange { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public TimeRange Comparison { get; set; }

        [Required]
        public TimeRange Granularity { get; set; }

        [Required]
        public eChartType ChartType { get; set; }

        public IList<ChartValueModel> Values { get; set; }
    }
}