using DREAM.Attributes;
using DREAM.Helpers;
using DREAM.Models;
using DREAM.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace DREAM.Controllers
{
    public class ReportsController : Controller
    {
        private DREAMContext db = new DREAMContext();

        [HttpGet]
        public ActionResult Generate()
        {
            populateDropdownLists();

            return View();
        }

        [HttpPost]
        public ActionResult Generate(ReportModel report)
        {
            if (ModelState.IsValid)
            {
                TestData.Initialize();
                List<Request> testRequests = TestData.Requests;
                List<Question> testQuestions = TestData.Questions;

                using (ExcelPackage package = new ExcelPackage())
                {
                    dumpRawData(package, testRequests);
                    dumpRawData(package, testQuestions);

                    if (report.Charts.Any(chart => chart.ObjectType == typeof(Request)))
                    {
                        dumpChartData(package, report, testRequests);
                    }
                    if (report.Charts.Any(chart => chart.ObjectType == typeof(Question)))
                    {
                        dumpChartData(package, report, testQuestions);
                    }

                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report.Name + ".xlsx");
                }
            }

            populateDropdownLists();

            return View();
        }

        private void dumpRawData<ObjectType>(ExcelPackage package, List<ObjectType> objects)
        {
            Type objectType = typeof(ObjectType);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Raw " + objectType.Name + " Data");

            List<string> headers = new List<string>();
            List<MemberInfo> members = new List<MemberInfo>();

            foreach (MemberInfo member in objectType.GetProperties())
            {
                ReportableAttribute attribute = (ReportableAttribute)Attribute.GetCustomAttribute(member, typeof(ReportableAttribute));
                if (attribute != null && attribute.Reportable)
                {
                    headers.Add(attribute.Name ?? member.Name);
                    members.Add(member);
                }
            }

            ExcelRangeBase headerCells = worksheet.Cells["A1"].LoadFromArrays(new object[][] { headers.ToArray() });
            object[][] data = DataExtractor.GetRawData(objects, members);
            worksheet.Cells["A2"].LoadFromArrays(data.ToArray());

            headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            for (int i = 1; i <= members.Count; i++)
            {
                MemberInfo member = members[i - 1];
                Type memberType = null;

                if(member is PropertyInfo)
                {
                    memberType = ((PropertyInfo)member).PropertyType;
                }
                else if(member is FieldInfo)
                {
                    memberType = ((FieldInfo)member).FieldType;
                }

                if(memberType == typeof(DateTime) || memberType == typeof(DateTime?))
                {
                    worksheet.Column(i).Style.Numberformat.Format = "DD-MMM-YY";
                }
            }
        }

        private void dumpChartData<ObjectType>(ExcelPackage package, ReportModel report, List<ObjectType> objects) where ObjectType : IReportable
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Chart Data (" + typeof(ObjectType).Name + ")");
            int row = 1;

            foreach (ChartModel chart in report.Charts)
            {
                if (chart.ObjectType != typeof(ObjectType))
                {
                    continue;
                }

                ExcelRange cells = addData(worksheet, row, chart, objects);
                ExcelWorksheet chartWorksheet = package.Workbook.Worksheets.Add(chart.Name);
                addChart(chartWorksheet, worksheet, row, cells.End.Row, cells.End.Column, chart.Name, chart.ChartType);

                row += cells.End.Row + 1;
            }
        }

        private ExcelRange addData<ObjectType>(ExcelWorksheet worksheet, int startingRow, ChartModel chart, List<ObjectType> objects) where ObjectType : IReportable
        {
            int titleRow = startingRow;
            int headerRow = startingRow + 1;
            int firstDataRow = startingRow + 2;

            ExcelRange cells;
            string timeRangeName = getTimeRangeName(chart.TimeRange, chart.StartDate);
            List<string> sectionHeadings = getSectionHeadings(chart);

            worksheet.Cells[titleRow, 1].Value = chart.Name;
            worksheet.Cells[headerRow, 1].Value = timeRangeName;
            cells = addDataForTimeRange(worksheet, firstDataRow, chart, objects);

            if (chart.Comparison != TimeRange.NONE)
            {
                DateTime comparisonStartDate = TimeRangeStepper.DecrementByGranularity(chart.StartDate, chart.Comparison);
                ChartModel comparisonChart = new ChartModel
                {
                    TimeRange = chart.TimeRange,
                    StartDate = comparisonStartDate,
                    Granularity = chart.Granularity,
                    Values = (IList<ChartValueModel>)chart.Values.Select(v => new ChartValueModel
                    {
                        Name = v.Name + " (" + getTimeRangeName(chart.TimeRange, comparisonStartDate) + ")",
                        Function = v.Function,
                        PropertyName = v.PropertyName,
                    }).ToList()
                };

                List<string> comparisonHeadings = getSectionHeadings(comparisonChart);

                if (comparisonHeadings.Count > sectionHeadings.Count)
                {
                    sectionHeadings = comparisonHeadings;
                }
                cells = addDataForTimeRange(worksheet, cells.End.Row + 1, comparisonChart, objects);
            }

            worksheet.Cells[headerRow, 2].LoadFromArrays(new object[][] { sectionHeadings.ToArray() });

            return worksheet.Cells[startingRow, 1, cells.End.Row, cells.End.Column];
        }

        private ExcelRange addDataForTimeRange<ObjectType>(ExcelWorksheet worksheet, int firstDataRow, ChartModel chart, List<ObjectType> objects) where ObjectType : IReportable
        {
            List<object[]> data = new List<object[]>();
            List<string> valueNames = new List<string>();

            foreach (ChartValueModel value in chart.Values)
            {
                foreach (KeyValuePair<string, List<object>> row in DataExtractor.GetDataRows(chart, objects,
                    value.GetMemberFor(typeof(ObjectType)), value.Function, chart.GetStratificationMemberFor(typeof(ObjectType))))
                {
                    string stratification = row.Key;
                    if (stratification == "")
                    {
                        valueNames.Add(value.Name);
                    }
                    else
                    {
                        valueNames.Add(value.Name + " (" + stratification + ")");
                    }
                    data.Add(row.Value.ToArray());
                }
            }

            worksheet.Cells[firstDataRow, 1].LoadFromCollection(valueNames);
            ExcelRangeBase dataCells = worksheet.Cells[firstDataRow, 2].LoadFromArrays(data.ToArray());

            return worksheet.Cells[firstDataRow, 1, dataCells.End.Row, dataCells.End.Column];
        }

        private void addChart(ExcelWorksheet chartWorksheet, ExcelWorksheet dataWorksheet, int startRow, int numRows, int numColumns,
            string chartName, eChartType chartType)
        {
            ExcelChart chart = chartWorksheet.Drawings.AddChart(chartName, chartType);
            chart.SetPosition(1, 0, 1, 0);
            chart.SetSize(100);

            int headerRow = startRow + 1;
            int firstDataRow = startRow + 2;

            for (int row = firstDataRow; row <= numRows; row++)
            {
                ExcelChartSerie serie = chart.Series.Add(dataWorksheet.Cells[row, 2, row, numColumns],
                    dataWorksheet.Cells[headerRow, 2, headerRow, numColumns]);
                serie.HeaderAddress = dataWorksheet.Cells[row, 1];
            }

            chart.Title.Text = chartName;
        }

        private string getTimeRangeName(TimeRange timeRange, DateTime startDate)
        {
            string dateFormat = "";

            switch (timeRange)
            {
                case TimeRange.ALL_TIME:
                    return "All Time";
                case TimeRange.DAY:
                    dateFormat = "d";
                    break;
                case TimeRange.HOUR:
                    dateFormat = "H";
                    break;
                case TimeRange.MONTH:
                    dateFormat = "MMMM";
                    break;
                case TimeRange.WEEK:
                    return (startDate.Date.Day / 7 + 1).ToString();
                case TimeRange.YEAR:
                    dateFormat = "yyyy";
                    break;
            }

            return startDate.ToString(dateFormat);
        }

        private List<string> getSectionHeadings(ChartModel chart)
        {
            List<string> headings = new List<string>();

            for (TimeRangeStepper stepper = new TimeRangeStepper(chart);
                stepper.CurrentStartDate < stepper.EndDate; stepper.Step())
            {
                headings.Add(getTimeRangeName(chart.Granularity, stepper.CurrentStartDate));
            }

            return headings;
        }

        #region Helpers
        private void populateDropdownLists()
        {
            ViewBag.TimeRangeList = buildTimeRangeDropdownList();
            ViewBag.StatFunctionList = buildStatFunctionDropdownList();
            ViewBag.ChartTypeList = buildChartTypeDropdownList();
            ViewBag.ComparisonList = buildComparisonDropdownList();
            ViewBag.RequestPropertiesList = buildPropertiesDropdownList(typeof(Request));
            ViewBag.RequestStratificationList = buildStratificationDropdownList(typeof(Request));
            ViewBag.ObjectTypeList = buildObjectTypeDropdownList();
            ViewBag.QuestionPropertiesList = buildPropertiesDropdownList(typeof(Question));
            ViewBag.QuestionStratificationList = buildStratificationDropdownList(typeof(Question));

            ViewBag.RequestPropertyOptions = (new JavaScriptSerializer()).Serialize(ViewBag.RequestPropertiesList);
            ViewBag.RequestStratificationOptions = (new JavaScriptSerializer()).Serialize(ViewBag.RequestStratificationList);
            ViewBag.QuestionPropertyOptions = (new JavaScriptSerializer()).Serialize(ViewBag.QuestionPropertiesList);
            ViewBag.QuestionStratificationOptions = (new JavaScriptSerializer()).Serialize(ViewBag.QuestionStratificationList);
        }

        private IEnumerable<SelectListItem> buildTimeRangeDropdownList()
        {
            List<SelectListItem> timeRanges = new List<SelectListItem>();

            timeRanges.Add(new SelectListItem { Text = "Hour", Value = TimeRange.HOUR.ToString() });
            timeRanges.Add(new SelectListItem { Text = "Day", Value = TimeRange.DAY.ToString() });
            timeRanges.Add(new SelectListItem { Text = "Week", Value = TimeRange.WEEK.ToString() });
            timeRanges.Add(new SelectListItem { Text = "Month", Value = TimeRange.MONTH.ToString() });
            timeRanges.Add(new SelectListItem { Text = "Year", Value = TimeRange.YEAR.ToString() });
            timeRanges.Add(new SelectListItem { Text = "All Time", Value = TimeRange.ALL_TIME.ToString() });

            return timeRanges;
        }

        private IEnumerable<SelectListItem> buildComparisonDropdownList()
        {
            List<SelectListItem> comparisons = new List<SelectListItem>
            {
                new SelectListItem { Text="None", Value=null }
            };

            comparisons.AddRange(buildTimeRangeDropdownList().Where(tr => tr.Value != TimeRange.ALL_TIME.ToString()).Select(
                tr => new SelectListItem { Text = "Previous " + tr.Text, Value = tr.Value }));

            return comparisons;
        }

        private IEnumerable<SelectListItem> buildStatFunctionDropdownList()
        {
            List<SelectListItem> statFunctions = new List<SelectListItem>();

            statFunctions.Add(new SelectListItem { Text = "Sum", Value = StatFunction.SUM.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Maximum", Value = StatFunction.MAX.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Minimum", Value = StatFunction.MIN.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Average", Value = StatFunction.AVG.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Count", Value = StatFunction.COUNT.ToString() });

            return statFunctions;
        }

        private IEnumerable<SelectListItem> buildChartTypeDropdownList()
        {
            List<SelectListItem> chartTypes = new List<SelectListItem>();

            chartTypes.Add(new SelectListItem { Text = "Line", Value = eChartType.Line.ToString() });
            chartTypes.Add(new SelectListItem { Text = "Clustered Column", Value = eChartType.ColumnClustered.ToString() });

            return chartTypes;
        }

        private IEnumerable<SelectListItem> buildPropertiesDropdownList(Type type)
        {
            List<SelectListItem> properties = new List<SelectListItem>
            {
                new SelectListItem { Text = "-----", Value = "" }
            };
            string propertyDisplayName;
            string propertyName;
            Attribute[] attributes;
            ChartableAttribute reportableAttribute;

            foreach (MemberInfo member in type.GetProperties())
            {
                if (!Attribute.IsDefined(member, typeof(ChartableAttribute)))
                {
                    continue;
                }

                attributes = Attribute.GetCustomAttributes(member);
                reportableAttribute = (ChartableAttribute)attributes.Where(a => a is ChartableAttribute).First();
                propertyName = member.Name;
                propertyDisplayName = reportableAttribute.Name ?? propertyName;

                properties.Add(new SelectListItem { Text = propertyDisplayName, Value = propertyName });
            }

            return properties;
        }

        private Dictionary<string, string> buildPropertiesOptionsList(Type type)
        {
            Dictionary<string, string> propertyOptions = new Dictionary<string, string>
            {
                {"-----", "" },
            };

            string propertyDisplayName;
            string propertyName;
            Attribute[] attributes;
            ChartableAttribute reportableAttribute;

            foreach (MemberInfo member in type.GetProperties())
            {
                if (!Attribute.IsDefined(member, typeof(ChartableAttribute)))
                {
                    continue;
                }

                attributes = Attribute.GetCustomAttributes(member);
                reportableAttribute = (ChartableAttribute)attributes.Where(a => a is ChartableAttribute).First();
                propertyName = member.Name;
                propertyDisplayName = reportableAttribute.Name ?? propertyName;

                propertyOptions[propertyDisplayName] = propertyName;
            }

            return propertyOptions;
        }

        private List<SelectListItem> buildStratificationDropdownList(Type type)
        {
            List<SelectListItem> stratifications = new List<SelectListItem>
            {
                new SelectListItem { Text = "-----", Value = "" }
            };

            string propertyDisplayName;
            string propertyName;
            Attribute[] attributes;
            StratifiableAttribute stratifiableAttribute;

            foreach (MemberInfo member in type.GetProperties())
            {
                if (!Attribute.IsDefined(member, typeof(StratifiableAttribute)))
                {
                    continue;
                }

                attributes = Attribute.GetCustomAttributes(member);
                stratifiableAttribute = (StratifiableAttribute)attributes.Where(a => a is StratifiableAttribute).First();
                propertyName = member.Name;
                propertyDisplayName = stratifiableAttribute.Name ?? propertyName;

                stratifications.Add(new SelectListItem { Text = propertyDisplayName, Value = propertyName });
            }

            return stratifications;
        }

        private IEnumerable<SelectListItem> buildObjectTypeDropdownList()
        {
            List<SelectListItem> objectTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "-----", Value = "" }
            };

            objectTypes.Add(new SelectListItem { Text = "Request", Value = "Request" });
            objectTypes.Add(new SelectListItem { Text = "Question", Value = "Question" });

            return objectTypes;
        }
        #endregion
    }
}
