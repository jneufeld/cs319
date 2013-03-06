using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class Question
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string QuestionText { get; set; }
        public int TimeTaken { get; set; }
        public string Response { get; set; }
        public int Probability { get; set; }
        public int Severity { get; set; }
        public string SpecialNotes { get; set; }
        public TumourGroup TumourGroup { get; set; }
        public int RequestID { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<Reference> Reference { get; set; }
    }
}