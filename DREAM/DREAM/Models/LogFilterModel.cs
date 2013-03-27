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
        [Display(Name = "Request ID", Prompt = "Leave blank to search all requests")]
        public int? RequestID { get; set; }

        [Display(Name = "Username", Prompt = "Leave blank to search all users")]
        public String UserName { get; set; }

        [Display(Name = "Action", Prompt = "Leave blank to search all actions")]
        public String Action { get; set; }

        [Display(Name = "Before This Date", Prompt = "Leave blank to unbound bottom of date filter")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Before { get; set; }

        [Display(Name = "After This Date", Prompt = "Leave blank to unbound top of date filter")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? After { get; set; }

        public int page;
        public IEnumerable<Log> Logs { get; set; }
        public IPagedList<Log> PagedLogs { get; set; }

        public LogFilterModel()
        {
            DREAMContext db = new DREAMContext();

            this.Logs = db.Logs.ToList();
            this.page = 1;
        }

        public LogFilterModel(int? request, String username, String actn, DateTime? before, DateTime? after, int page)
        {
            DREAMContext db = new DREAMContext();

            this.Logs = db.Logs.ToList();

            this.RequestID = request;
            this.UserName = username;
            this.Action = actn;
            this.Before = before;
            this.After = after;
            this.page = page;
        }

        public void filter()
        {
            if (this.RequestID != null)
            {
                this.Logs = this.Logs.Where(log => log.RequestID == this.RequestID);
            }

            if (this.UserName != null)
            {
                this.Logs = this.Logs.Where(log => log.User.UserName.Equals(this.UserName));
            }

            if (this.Action != null)
            {
                this.Logs = this.Logs.Where(log => log.Action == Convert.ToInt32(this.Action));
            }

            if (this.Before != null)
            {
                this.Logs = this.Logs.Where(log => log.TimePerformed.Date.CompareTo(this.Before.Value.Date) < 0);
            }

            if (this.After != null)
            {
                this.Logs = this.Logs.Where(log => log.TimePerformed.Date.CompareTo(this.After.Value.Date) > 0);
            }

            this.PagedLogs = this.Logs.OrderByDescending(m => m.ID).ToPagedList(page, 10);
        }
    }
}