using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class LogFilterModel
    {
        [Display(Name = "Request ID")]
        public int? RequestID { get; set; }
        public Guid? UserID { get; set; }
        public LogAction? Action { get; set; }
        public DateTime? Before { get; set; }
        public DateTime? After { get; set; }
        public IEnumerable<Log> Logs { get; set; }
        public IPagedList<Log> PagedLogs { get; set; }

        public LogFilterModel()
        {
            DREAMContext db = new DREAMContext();

            Logs = db.Logs.ToList();
        }
    }
}