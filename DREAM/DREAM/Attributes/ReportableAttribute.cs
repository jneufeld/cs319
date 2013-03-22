using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Attributes
{
    public class ReportableAttribute : Attribute
    {
        public string Name = null;
        public StatFunction[] StatFunctions = new StatFunction[] { StatFunction.AVG, StatFunction.COUNT, StatFunction.MAX, StatFunction.MIN, StatFunction.SUM };

        public ReportableAttribute() { }

        public ReportableAttribute(string name)
        {
            Name = name;
        }
    }
}