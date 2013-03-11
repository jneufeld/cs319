using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DREAM.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Request ID")]
        public int ID { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Date Completed")]
        public DateTime? CompletionTime { get; set; }

        [Display(Name = "Request Type")]
        public RequestType Type { get; set; }

        public Caller Caller { get; set; }
        public Patient Patient { get; set; }

        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Display(Name = "Closed By")]
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
    }
}