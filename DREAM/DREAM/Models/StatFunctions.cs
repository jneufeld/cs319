using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public enum StatFunction
    {
        [Description("Sum")]
        SUM = 0,
        [Description("Maximum")]
        MAX = 1,
        [Description("Minimum")]
        MIN = 2,
        [Description("Average")]
        AVG = 3,
        [Description("Count")]
        COUNT = 4,
    }
}