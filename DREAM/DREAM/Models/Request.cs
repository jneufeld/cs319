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
        public int ID { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public RequestType Type { get; set; }
        public Caller CallerID { get; set; }
        public Patient PatientID { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ClosedBy { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}