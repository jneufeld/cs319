using DREAM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class Question : IReportable
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Chartable("Total Questions", StatFunctions = new StatFunction[] {StatFunction.COUNT}, Reportable=false)]
        public int ID { get; set; }
        public string QuestionText { get; set; }
        [Chartable("Time Taken")]
        public int TimeTaken { get; set; }
        public string Response { get; set; }
        [Reportable]
        public int Probability { get; set; }
        [Stratifiable]
        public QuestionType QuestionType { get; set; }
        [Reportable]
        public int Severity { get; set; }
        public string SpecialNotes { get; set; }
        [Stratifiable]
        public TumourGroup TumourGroup { get; set; }
        public Request Request { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<Reference> References { get; set; }

        [Reportable("Request's Received Date")]
        public DateTime CreationTime
        {
            get
            {
                return Request.CreationTime;
            }
        }

        [Reportable("Request's Closed Date")]
        public DateTime? CompletionTime
        {
            get
            {
                return Request.CompletionTime;
            }
        }
    }
}