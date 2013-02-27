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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID;
        public DateTime CreationTime;
        public DateTime CompletionTime;
        public RequestType Type;
        public Caller Caller;
        public Patient Patient;
        public Guid CreatedBy;
        public Guid ClosedBy;
        //virtual ICollection<Question> Questions { get; set; }
    }
}