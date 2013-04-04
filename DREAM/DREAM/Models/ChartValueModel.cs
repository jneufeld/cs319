using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DREAM.Models
{
    public class ChartValueModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Property")]
        public string PropertyName { get; set; }

        [Required]
        public StatFunction Function { get; set; }

        public MemberInfo GetMemberFor(Type t)
        {
            if (PropertyName!=null)
            {
                return t.GetProperty(PropertyName);
            }
            return null;
        }
    }
}