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
        public static List<KeyValuePair<string, List<object>>> GetDataRows<ObjectType>(ChartModel chart, IEnumerable<ObjectType> objects,
            MemberInfo member, StatFunction function, MemberInfo stratificationMember = null) where ObjectType:IReportable
        {
            Dictionary<string, List<object>> dataRows = new Dictionary<string, List<object>>();

            for (TimeRangeStepper stepper = new TimeRangeStepper(chart);
                stepper.CurrentStartDate < stepper.EndDate; stepper.Step())
            {
                IEnumerable<ObjectType> currentObjects = objects.Where(
                    obj => obj.CreationTime >= stepper.CurrentStartDate && obj.CreationTime < stepper.CurrentEndDate);
                IEnumerable<IGrouping<object, ObjectType>> groupedRequests = getStratifiedData(currentObjects, stratificationMember);

                IEnumerable<Tuple<string, object>> stratifiedValues;

                if (chart.Stratification == null)
                {
                    Type t = ((PropertyInfo)member).PropertyType;
                    stratifiedValues = new List<Tuple<string, object>>{
                        new Tuple<string, object>("", performStatFunction(function, currentObjects, member))
                    };
                }
                else
                {
                    stratifiedValues = groupedRequests.Select(group => new Tuple<string, object>(group.Key.ToString(),
                        (int)performStatFunction(function, group, member)));
                }

                foreach(Tuple<string, object> stratifiedValue in stratifiedValues)
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
                    while (row.Count <= stepper.StepCount)
                    {
                        row.Add(0);
                    }
                }
            }

            return dataRows.ToList();
        }

        public static object[][] GetRawData<ObjectType>(IEnumerable<ObjectType> objects, List<MemberInfo> members)
        {
            return objects.Select(obj => getValues(obj, members)).ToArray();
        }

        private static object[] getValues<ObjectType>(ObjectType obj, List<MemberInfo> members)
        {
            return members.Select(member => getValue(obj, member)).ToArray();
        }

        private static IEnumerable<IGrouping<object, ObjectType>> getStratifiedData<ObjectType>(
            IEnumerable<ObjectType> currentObjects, MemberInfo stratificationMember)
        {
            if (stratificationMember == null)
            {
                return null;
            }

            if (stratificationMember is PropertyInfo)
            {
                return currentObjects.GroupBy(obj => ((PropertyInfo)stratificationMember).GetValue(obj, null));
            }
            else if (stratificationMember is FieldInfo)
            {
                return currentObjects.GroupBy(obj => ((FieldInfo)stratificationMember).GetValue(obj));
            }
            else
            {
                return null;
            }
        }

        private static object getValue<ObjectType>(ObjectType obj, MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return ((PropertyInfo)member).GetValue(obj, null);
            }
            else if (member is FieldInfo)
            {
                return ((FieldInfo)member).GetValue(obj);
            }
            else
            {
                return obj;
            }
        }

        private static object performStatFunction<ObjectType>(StatFunction function, IEnumerable<ObjectType> objects,
            MemberInfo member)
        {
            switch (function)
            {
                case StatFunction.AVG:
                    return objects.Average(obj => (int)getValue(obj, member));
                case StatFunction.SUM:
                    return objects.Sum(obj => (int)getValue(obj, member));
                case StatFunction.COUNT:
                    return objects.Count();
                case StatFunction.MAX:
                    return objects.Max(obj => (int)getValue(obj, member));
                case StatFunction.MIN:
                    return objects.Min(obj => (int)getValue(obj, member));
                default:
                    throw new Exception("Unknown stat function '" + function.ToString() + "'.");
            }
        }
    }
}