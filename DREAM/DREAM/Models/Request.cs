using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DREAM.Models
{
    public class Request
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Display(Name = "Request ID")]
        public int ID { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Date Completed")]
        public DateTime? CompletionTime { get; set; }

        [Display(Name = "Request Type")]
        public RequestType Type { get; set; }
        public Caller CallerID { get; set; }
        public Patient PatientID { get; set; }

        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Display(Name = "Closed By")]
        public Guid ClosedBy { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}