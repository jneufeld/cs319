using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Attributes
{
    public class ChartableAttribute : ReportableAttribute
    {
        public StatFunction[] StatFunctions = new StatFunction[] { StatFunction.AVG, StatFunction.MAX, StatFunction.MIN, StatFunction.SUM };

        public ChartableAttribute() : base() { }

        public ChartableAttribute(string name) : base(name) { }
    }
}