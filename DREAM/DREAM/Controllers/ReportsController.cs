using DREAM.Models;
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
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DREAM.Controllers
{
    public class ReportsController : Controller
    {
        private DREAMContext db = new DREAMContext();

        private const char REQUEST_RECEIVED_DATE_COLUMN = 'A';
        private const char REQUEST_REQUESTOR_TYPE_COLUMN = 'B';
        private const char REQUEST_REGION_COLUMN = 'C';
        private const char REQUEST_TIME_SPENT_COLUMN = 'D';
        private const char REQUEST_CLOSED_DATE_COLUMN = 'E';
        private const char REQUEST_PHARMACIST_COLUMN = 'F';

        [HttpGet]
        public ActionResult Generate()
        {
            ViewBag.TimeRangeList = BuildTimeRangeDropdownList();
            ViewBag.StatFunctionList = BuildStatFunctionDropdownList();
            ViewBag.ChartTypeList = BuildChartTypeDropdownList();

            return View();
        }

        [HttpPost]
        public ActionResult Generate(ReportModel report)
        {
            if (ModelState.IsValid)
            {
                ReportModel testReport = new ReportModel
                {
                    Name = "Test Report",
                    Charts = new List<ChartModel>
                    {
                        new ChartModel
                        {
                            Name = "Requests and total time",
                            StartDate = new DateTime(2013, 1, 1),
                            TimeRange = TimeRange.YEAR,
                            Granularity = TimeRange.MONTH,
                            Comparison = TimeRange.NONE,
                            ChartType = eChartType.Line,
                            Values = new List<ChartValueModel>
                            {
                                new ChartValueModel
                                {
                                    Name = "Total requests",
                                    Function = StatFunction.COUNT,
                                    PropertyName = null,
                                },
                                new ChartValueModel
                                {
                                    Name = "Total time (mins)",
                                    Function = StatFunction.SUM,
                                    PropertyName = "TimeSpent",
                                }
                            }
                        },
                        new ChartModel
                        {
                            Name = "Request and total time (bar)",
                            StartDate = new DateTime(2013, 1, 1),
                            TimeRange = TimeRange.YEAR,
                            Granularity = TimeRange.MONTH,
                            Comparison = TimeRange.NONE,
                            ChartType = eChartType.ColumnClustered,
                            Values = new List<ChartValueModel>
                            {
                                new ChartValueModel
                                {
                                    Name = "Total requests",
                                    Function = StatFunction.COUNT,
                                    PropertyName = null,
                                },
                                new ChartValueModel
                                {
                                    Name = "Total time (mins)",
                                    Function = StatFunction.SUM,
                                    PropertyName = "TimeSpent",
                                }
                            }
                        },
                    }
                };

                Caller testCaller = new Caller
                {
                    Email = "test@example.com",
                    FirstName = "First",
                    LastName = "Last",
                    PhoneNumber = "555 555-5555",
                    Region = db.Regions.Find(1),
                };

                MembershipUser testUser = Membership.GetUser("user1");
                List<Request> testRequests = new List<Request>{
                    new Request{ID=0, CreationTime=DateTime.Now, CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(1), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller,
                        Questions=new List<Question>{
                            new Question{TimeTaken=20},
                            new Question{TimeTaken=50},
                        }},
                    new Request{ID=2, CreationTime=new DateTime(2013, 1, 1), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(2), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller,
                        Questions=new List<Question>{
                            new Question{TimeTaken=60},
                            new Question{TimeTaken=50},
                        }},
                    new Request{ID=3, CreationTime=new DateTime(2013, 1, 21), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(2), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller},
                    new Request{ID=4, CreationTime=new DateTime(2013, 2, 28), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(1), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller,
                        Questions=new List<Question>{
                            new Question{TimeTaken=20},
                            new Question{TimeTaken=50},
                        }},
                    new Request{ID=5, CreationTime=new DateTime(2013, 3, 4), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(2), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller,
                        Questions=new List<Question>{
                            new Question{TimeTaken=60},
                            new Question{TimeTaken=50},
                        }},
                    new Request{ID=6, CreationTime=new DateTime(2013, 12, 25), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(2), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller},
                    new Request{ID=7, CreationTime=new DateTime(2013, 10, 1), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(1), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller,
                        Questions=new List<Question>{
                            new Question{TimeTaken=20},
                            new Question{TimeTaken=50},
                        }},
                    new Request{ID=8, CreationTime=new DateTime(2013, 5, 16), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(2), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller,
                        Questions=new List<Question>{
                            new Question{TimeTaken=60},
                            new Question{TimeTaken=50},
                        }},
                    new Request{ID=9, CreationTime=new DateTime(2013, 3, 9), CompletionTime=DateTime.Now,
                        Type=db.RequestTypes.Find(2), CreatedBy=(Guid)testUser.ProviderUserKey,
                        Caller=testCaller},
                };

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet requests = package.Workbook.Worksheets.Add("Raw Request Data");
                    ExcelWorksheet questions = package.Workbook.Worksheets.Add("Raw Question Data");

                    dumpRawRequestData(requests, testRequests);
                    dumpStratifiedRequestData(package, report, testRequests);

                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report.Name + ".xlsx");
                }
            }

            ViewBag.TimeRangeList = BuildTimeRangeDropdownList();
            ViewBag.StatFunctionList = BuildStatFunctionDropdownList();
            ViewBag.ChartTypeList = BuildChartTypeDropdownList();

            return View();
        }

        private void dumpRawRequestData(ExcelWorksheet requests, List<Request> testRequests)
        {
            List<RequestRow> requestData = new List<RequestRow>();
            foreach (Request request in testRequests)
            {
                int timeSpent = 0;
                foreach (Question question in request.Questions)
                {
                    timeSpent += question.TimeTaken;
                }

                requestData.Add(new RequestRow
                {
                    Closed_Date = request.CompletionTime,
                    Received_Date = request.CreationTime,
                    Region = request.Caller.Region.FullName,
                    Time_Spent = timeSpent,
                    Requester_Type = request.Type.FullName,
                    Pharmacist = Membership.GetUser(request.CreatedBy).UserName,
                });
            };


            ExcelRangeBase requestCells = requests.Cells["A1"].LoadFromCollection(requestData, true);

            using (ExcelRange requestsHeader = requests.Cells[requestCells.Start.Row, requestCells.Start.Column, 
                requestCells.Start.Row, requestCells.End.Column])
            {
                requestsHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                requestsHeader.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            requests.Cells[
                REQUEST_RECEIVED_DATE_COLUMN + ":" + REQUEST_RECEIVED_DATE_COLUMN + "," +
                REQUEST_CLOSED_DATE_COLUMN + ":" + REQUEST_CLOSED_DATE_COLUMN
                ].Style.Numberformat.Format = "DD-MMM-YY";
        }

        private void dumpStratifiedRequestData(ExcelPackage package, ReportModel report, List<Request> requests)
        {
            ExcelWorksheet stratifiedRequests = package.Workbook.Worksheets.Add("Stratified Request Data");
            int row = 1;

            foreach (ChartModel chart in report.Charts)
            {
                ExcelRange cells = addDataForTimeRange(stratifiedRequests.Cells[row, 1], chart.TimeRange, chart.StartDate,
                    chart.Granularity, chart.Values, requests);
                ExcelWorksheet chartWorksheet = package.Workbook.Worksheets.Add(chart.Name);
                addChart(chartWorksheet, stratifiedRequests, row, cells.End.Row, cells.End.Column, chart.Name, chart.ChartType);

                row += cells.End.Row + 1;
            }
        }

        private ExcelRange addDataForTimeRange(ExcelRange startCell, TimeRange timeRange, DateTime startTime, TimeRange granularity, IList<ChartValueModel> values, List<Request> requests)
        {
            string sectionTitle = getSectionTitle(timeRange, startTime);
            startCell[1, 1].Value = sectionTitle;
            List<string> sectionHeadings = getSectionHeadings(timeRange, startTime, granularity);
            ExcelRangeBase headers = startCell[1, 2].LoadFromArrays(new object[][]{sectionHeadings.ToArray()});

            startCell[2, 1].LoadFromCollection(values.Select(v => v.Name));

            object[][] data = new object[values.Count()][];

            for(int i=0; i<values.Count(); i++)
            {
                data[i] = getRowData(timeRange, startTime, granularity, requests,
                    values[i].GetMemberFor(typeof(Request)), values[i].Function);
            }

            ExcelRangeBase dataCells = startCell[2, 2].LoadFromArrays(data);

            return startCell[1, 1, dataCells.End.Row, dataCells.End.Column];
        }

        private void addChart(ExcelWorksheet chartWorksheet, ExcelWorksheet dataWorksheet, int startRow, int numRows, int numColumns, string chartName, eChartType chartType)
        {
            ExcelChart chart = chartWorksheet.Drawings.AddChart(chartName, chartType);
            chart.SetPosition(1, 0, 1, 0);
            chart.SetSize(100);

            for (int row = 2; row <= numRows; row++)
            {
                ExcelChartSerie serie = chart.Series.Add(dataWorksheet.Cells[row, 2, row, numColumns],
                    dataWorksheet.Cells[1, 2, 1, numColumns]);
                serie.HeaderAddress = dataWorksheet.Cells[row, 1];
            }

            chart.Title.Text = chartName;
        }

        private string getSectionTitle(TimeRange timeRange, DateTime startTime)
        {
            switch (timeRange)
            {
                case TimeRange.ALL_TIME:
                    return "All Time";
                case TimeRange.DAY:
                    return startTime.Date.ToString();
                case TimeRange.HOUR:
                    return startTime.ToString();
                case TimeRange.MONTH:
                    return startTime.Date.ToString();
                case TimeRange.WEEK:
                    return startTime.Date.ToString();
                case TimeRange.YEAR:
                    return startTime.Year.ToString();
            }

            return "";
        }

        private List<string> getSectionHeadings(TimeRange timeRange, DateTime startDate, TimeRange granularity)
        {
            List<string> headings = new List<string>();

            for (TimeRangeStepper stepper = new TimeRangeStepper(timeRange, startDate, granularity);
                stepper.CurrentStartDate < stepper.EndDate; stepper.Step())
            {
                switch (granularity)
                {
                    case TimeRange.ALL_TIME:
                        throw new NotImplementedException();
                    case TimeRange.MONTH:
                        headings.Add(stepper.CurrentStartDate.ToString("MMMM"));
                        break;
                }
            }
            return headings;
        }

        private object[] getRowData(TimeRange timeRange, DateTime startTime, TimeRange granularity, List<Request> requests, MemberInfo member, StatFunction function)
        {
            List<object> rowData = new List<object>();

            for (TimeRangeStepper stepper = new TimeRangeStepper(timeRange, startTime, granularity); 
                stepper.CurrentStartDate < stepper.EndDate; stepper.Step())
            {
                IEnumerable<Request> currentRequests = requests.Where(
                    r => r.CreationTime >= stepper.CurrentStartDate && r.CreationTime < stepper.CurrentEndDate);
                IEnumerable<object> requestPropertyValues = currentRequests;

                if (member is PropertyInfo)
                {
                    requestPropertyValues = currentRequests.Select(r => ((PropertyInfo)member).GetValue(r, null));
                }
                else if (member is FieldInfo)
                {
                    requestPropertyValues = currentRequests.Select(r => ((FieldInfo)member).GetValue(r));
                }

                switch (function)
                {
                    case StatFunction.AVG:
                        rowData.Add(requestPropertyValues.Average(prop => (int)prop));
                        break;
                    case StatFunction.COUNT:
                        rowData.Add(requestPropertyValues.Count());
                        break;
                    case StatFunction.SUM:
                        rowData.Add(requestPropertyValues.Sum(prop => (int)prop));
                        break;
                }
            }

            return rowData.ToArray();
        }

        #region Helpers
        private IEnumerable<SelectListItem> BuildTimeRangeDropdownList()
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

        private IEnumerable<SelectListItem> BuildStatFunctionDropdownList()
        {
            List<SelectListItem> statFunctions = new List<SelectListItem>();

            statFunctions.Add(new SelectListItem { Text = "Sum", Value = StatFunction.SUM.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Maximum", Value = StatFunction.MAX.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Minimum", Value = StatFunction.MIN.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Average", Value = StatFunction.AVG.ToString() });
            statFunctions.Add(new SelectListItem { Text = "Count", Value = StatFunction.COUNT.ToString() });

            return statFunctions;
        }

        private IEnumerable<SelectListItem> BuildChartTypeDropdownList()
        {
            List<SelectListItem> chartTypes = new List<SelectListItem>();

            chartTypes.Add(new SelectListItem { Text = "Line", Value = eChartType.Line.ToString() });
            chartTypes.Add(new SelectListItem { Text = "Clustered Column", Value = eChartType.ColumnClustered.ToString() });

            return chartTypes;
        }
        #endregion

        public class TimeRangeStepper
        {
            public TimeRange TimeRange;
            public DateTime StartDate;
            public DateTime EndDate;
            public TimeRange Granularity;
            public DateTime CurrentStartDate;
            public DateTime CurrentEndDate;

            public TimeRangeStepper(TimeRange timeRange, DateTime startDate, TimeRange granularity)
            {
                TimeRange = timeRange;
                StartDate = startDate;
                EndDate = incrementByGranularity(startDate, timeRange);
                Granularity = granularity;
                CurrentStartDate = StartDate;
                CurrentEndDate = incrementByGranularity(StartDate);
            }

            private DateTime incrementByGranularity(DateTime dateTime)
            {
                return incrementByGranularity(dateTime, Granularity);
            }

            private DateTime incrementByGranularity(DateTime dateTime, TimeRange granularity)
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

        public class RequestRow
        {
            public DateTime Received_Date { get; set; }

            public string Requester_Type { get; set; }

            public string Region { get; set; }

            // public string Tumour_Group { get; set; }
            public int Time_Spent { get; set; }

            public DateTime? Closed_Date { get; set; }

            public string Pharmacist { get; set; }

            // public int Counter { get; set; }
            // public string Code { get; set; }
        }
    }
}
