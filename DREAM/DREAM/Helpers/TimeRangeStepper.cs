using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Helpers
{
    public class TimeRangeStepper
    {
        public TimeRange TimeRange;
        public DateTime StartDate;
        public DateTime EndDate;
        public TimeRange Granularity;
        public DateTime CurrentStartDate;
        public DateTime CurrentEndDate;

        public TimeRangeStepper(ChartModel chart)
        {
            TimeRange = chart.TimeRange;
            StartDate = chart.StartDate;
            EndDate = IncrementByGranularity(StartDate, TimeRange);
            Granularity = chart.Granularity;
            CurrentStartDate = StartDate;
            CurrentEndDate = incrementByGranularity(StartDate);
        }

        public static DateTime IncrementByGranularity(DateTime dateTime, TimeRange granularity)
        {
            switch (granularity)
            {
                case TimeRange.DAY:
                    return dateTime.AddDays(1);
                case TimeRange.HOUR:
                    return dateTime.AddHours(1);
                case TimeRange.MONTH:
                    return dateTime.AddMonths(1);
                case TimeRange.WEEK:
                    return dateTime.AddDays(7);
                case TimeRange.YEAR:
                     return dateTime.AddYears(1);
            }
            return dateTime;
        }

        public static DateTime DecrementByGranularity(DateTime dateTime, TimeRange granularity)
        {
            switch (granularity)
            {
                case TimeRange.DAY:
                    return dateTime.AddDays(-1);
                case TimeRange.HOUR:
                    return dateTime.AddHours(-1);
                case TimeRange.MONTH:
                    return dateTime.AddMonths(-1);
                case TimeRange.WEEK:
                    return dateTime.AddDays(-7);
                case TimeRange.YEAR:
                     return dateTime.AddYears(-1);
            }
            return dateTime;
        }

        private DateTime incrementByGranularity(DateTime dateTime)
        {
            return IncrementByGranularity(dateTime, Granularity);
        }

        public DateTime Step()
        {
            CurrentStartDate = incrementByGranularity(CurrentStartDate);
            CurrentEndDate = incrementByGranularity(CurrentStartDate);
            return CurrentStartDate;
        }

        public void Reset()
        {
            CurrentStartDate = StartDate;
            CurrentEndDate = incrementByGranularity(CurrentStartDate);
        }
    }
}