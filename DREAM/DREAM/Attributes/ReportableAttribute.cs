using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Attributes
{
    public class ReportableAttribute : Attribute
    {
        public string Name = null;
        public bool Reportable;

        public ReportableAttribute()
        {
            Reportable = true;
        }

        public ReportableAttribute(string name) : this()
        {
            Name = name;
        }
    }
}