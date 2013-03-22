using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DREAM.Attributes;

namespace DREAM.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ReportableAttribute("Total Requests", StatFunctions = new StatFunction[] {StatFunction.COUNT})]
        public int ID { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public RequestType Type { get; set; }

        public Caller Caller { get; set; }
        public Patient Patient { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid ClosedBy { get; set; }

        private ICollection<Question> _Questions;
        public virtual ICollection<Question> Questions
        {
            get { return _Questions ?? (_Questions = new HashSet<Question>()); }
            set { _Questions = value; }
        }

        public Lock Lock()
        {
            DREAMContext db = new DREAMContext();

            Lock reqLock = new Lock
            {
                ExpireTime = DateTime.Now.AddMinutes(1.0),
                UserID = (Guid)Membership.GetUser().ProviderUserKey,
                RequestID = this.ID,
            };

            db.Locks.Add(reqLock);
            db.SaveChanges();
            return reqLock;
        }

        public void Unlock()
        {
            DREAMContext db = new DREAMContext();

            Lock reqLock = db.Locks.Single(Lock => Lock.RequestID == this.ID);
            db.Locks.Remove(reqLock);
            db.SaveChanges();
        }

        [ReportableAttribute("Time Spent")]
        public int TimeSpent
        {
            get
            {
                return Questions.Sum(q => q.TimeTaken);
            }
        }
    }
}