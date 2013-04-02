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
    public class Request : IReportable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Chartable("Total Requests", StatFunctions = new StatFunction[] {StatFunction.COUNT}, Reportable=false)]
        public int ID { get; set; }

        public bool Enabled { get; set; }

        [Reportable("Received Date")]
        public DateTime CreationTime { get; set; }

        [Reportable("Closed Date")]
        public DateTime? CompletionTime { get; set; }

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

        [Chartable("Time Spent")]
        public int TimeSpent
        {
            get
            {
                return Questions.Sum(q => q.TimeTaken);
            }
        }

        [Stratifiable]
        public Region Region
        {
            get
            {
                return Caller.Region;
            }
        }

        [Stratifiable("Pharmacist")]
        public string Creator
        {
            get
            {
                MembershipUser user = Membership.GetUser(CreatedBy);
                return user != null ? user.UserName : "";
            }
        }

        [Stratifiable(Reportable=false)]
        public string Closer
        {
            get
            {
                MembershipUser user = Membership.GetUser(ClosedBy);
                return user != null ? user.UserName : "";
            }
        }

        [Stratifiable("Requester Type")]
        public RequesterType RequesterType
        {
            get
            {
                return Caller.Type;
            }
        }

        [Chartable("Number of Questions")]
        public int NumberOfQuestions
        {
            get
            {
                return Questions.Count();
            }
        }
    }
}