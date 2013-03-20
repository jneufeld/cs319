using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public enum TimeRange
    {
        NONE = -1,
        HOUR = 0,
        DAY = 1,
        WEEK = 2,
        MONTH = 3,
        YEAR = 4,
        ALL_TIME = 5,
    }
}