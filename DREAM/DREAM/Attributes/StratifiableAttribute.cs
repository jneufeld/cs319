using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Attributes
{
    public class StratifiableAttribute : ReportableAttribute
    {
        public StratifiableAttribute() : base() { }

        public StratifiableAttribute(string name) : base(name) { }
    }
}