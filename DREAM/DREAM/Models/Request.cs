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
        int ID;
        DateTime CreationTime;
        DateTime CompletionTime;
        RequestType Type;
        Caller Caller;
        Patient Patient;
        Guid CreatedBy;
        Guid ClosedBy;
        //virtual ICollection<Question> Questions { get; set; }
    }
}