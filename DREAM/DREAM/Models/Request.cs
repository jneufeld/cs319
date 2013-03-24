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
        public MembershipUser Creator
        {
            get
            {
                return Membership.GetUser(CreatedBy);
            }
        }

        [Stratifiable(Reportable=false)]
        public MembershipUser Closer
        {
            get
            {
                return Membership.GetUser((Guid)ClosedBy);
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