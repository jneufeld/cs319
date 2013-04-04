using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DREAM.Models
{
    public class ChartModel
    {
        [Required]
        [RegularExpression("(Request|Question)")]
        [Display(Name="Report Type")]
        public string ObjectTypeName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Time Range")]
        public TimeRange TimeRange { get; set; }

        [Required]
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        public TimeRange Comparison { get; set; }

        [Required]
        public TimeRange Granularity { get; set; }

        [Required]
        [Display(Name="Chart Type")]
        public eChartType ChartType { get; set; }

        public string Stratification { get; set; }

        public IList<ChartValueModel> Values { get; set; }

        public ChartModel()
        {
            TimeRange = TimeRange.YEAR;
            Granularity = TimeRange.MONTH;
            Comparison = TimeRange.NONE;
            ChartType = eChartType.Line;
        }

        public MemberInfo GetStratificationMemberFor(Type type)
        {
            if (Stratification != null)
            {
                return type.GetProperty(Stratification);
            }
            return null;
        }

        public Type ObjectType
        {
            get
            {
                switch (ObjectTypeName)
                {
                    case "Request":
                        return typeof(Request);
                    case "Question":
                        return typeof(Question);
                    default:
                        return null;
                }
            }
        }
    }
}