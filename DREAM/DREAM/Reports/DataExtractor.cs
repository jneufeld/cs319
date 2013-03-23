using DREAM.Helpers;
using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DREAM.Reports
{
    public class DataExtractor
    {
        public static List<KeyValuePair<string, List<object>>> GetDataRows(ChartModel chart, List<Request> requests, MemberInfo member,
            StatFunction function, MemberInfo stratificationMember = null)
        {
            Dictionary<string, List<object>> dataRows = new Dictionary<string, List<object>>();

            Func<IEnumerable<Request>, object> performStatFunction = getPerformStatFunctionFunc(function, member);

            for (TimeRangeStepper stepper = new TimeRangeStepper(chart);
                stepper.CurrentStartDate < stepper.EndDate; stepper.Step())
            {
                IEnumerable<Request> currentRequests = requests.Where(
                    r => r.CreationTime >= stepper.CurrentStartDate && r.CreationTime < stepper.CurrentEndDate);
                IEnumerable<IGrouping<object, Request>> groupedRequests = getStratifiedData(currentRequests, stratificationMember);

                IEnumerable<Tuple<string, int>> stratifiedValues;

                if (chart.Stratification == null)
                {
                    stratifiedValues = new List<Tuple<string, int>>{
                        new Tuple<string, int>("", (int)performStatFunction(currentRequests))
                    };
                }
                else
                {
                    stratifiedValues = groupedRequests.Select(group => new Tuple<string, int>(group.Key.ToString(), (int)performStatFunction(group)));
                }

                foreach(Tuple<string, int> stratifiedValue in stratifiedValues)
                {
                    string stratification = stratifiedValue.Item1;
                    if(!dataRows.ContainsKey(stratification))
                    {
                        dataRows.Add(stratification, new object[stepper.StepCount].ToList());
                    }

                    dataRows[stratification].Add(stratifiedValue.Item2);
                }

                foreach (List<object> row in dataRows.Values)
                {
                    while (row.Count < stepper.StepCount)
                    {
                        row.Add(0);
                    }
                }
            }

            return dataRows.ToList();
        }

        private static List<object> getUnstratifiedData()
        {
            List<object> data = new List<object>();
            return data;
        }

        private static IEnumerable<IGrouping<object, Request>> getStratifiedData(IEnumerable<Request> currentRequests,
            MemberInfo stratificationMember)
        {
            if (stratificationMember == null)
            {
                return null;
            }

            if (stratificationMember is PropertyInfo)
            {
                return currentRequests.GroupBy(r => ((PropertyInfo)stratificationMember).GetValue(r, null));
            }
            else if (stratificationMember is FieldInfo)
            {
                return currentRequests.GroupBy(r => ((FieldInfo)stratificationMember).GetValue(r));
            }
            else
            {
                return null;
            }
        }

        private static Func<Request, object> getValueFunc(MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return delegate(Request r) { return ((PropertyInfo)member).GetValue(r, null); };
            }
            else if (member is FieldInfo)
            {
                return delegate(Request r) { return ((FieldInfo)member).GetValue(r); };
            }
            else
            {
                return delegate(Request r) { return r; };
            }
        }

        private static Func<IEnumerable<Request>, object> getPerformStatFunctionFunc(StatFunction function, MemberInfo member)
        {
            Func<Request, object> getValue = getValueFunc(member);
            switch (function)
            {
                case StatFunction.AVG:
                    return delegate(IEnumerable<Request> requests)
                    {
                        return requests.Average(r => (int)getValue(r));
                    };
                case StatFunction.COUNT:
                    return delegate(IEnumerable<Request> requests)
                    {
                        return requests.Count();
                    };
                case StatFunction.SUM:
                    return delegate(IEnumerable<Request> requests)
                    {
                        return requests.Sum(r => (int)getValue(r));
                    };
                default:
                    return delegate(IEnumerable<Request> requests)
                    {
                        return requests.Count();
                    };
            }
        }
    }
}