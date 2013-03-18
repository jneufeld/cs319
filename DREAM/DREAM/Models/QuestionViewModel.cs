using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DREAM.Models
{
    public class QuestionViewModel
    {
        public int QuestionID { get; set; }
        public int Index { get; set; }

        public bool Delete { get; set; }

        [Display(Name = "Question")]
        public string QuestionText { get; set; }

        [Display(Name = "Time Taken")]
        public int TimeTaken { get; set; }

        [Display(Name = "Response")]
        public string Response { get; set; }

        [Display(Name = "Probability")]
        public int Probability { get; set; }

        [Display(Name = "Severity")]
        public int Severity { get; set; }

        [Display(Name = "Special Notes")]
        public string SpecialNotes { get; set; }

        [Display(Name = "Question Type")]
        public string QuestionTypeStringID { get; set; }
        public int QuestionTypeID
        {
            get
            {
                int tmp = 0; Int32.TryParse(QuestionTypeStringID, out tmp); return tmp;
            }
            set { QuestionTypeStringID = value.ToString(); }
        }

        [Display(Name = "Tumour Group")]
        public string TumourGroupStringID { get; set; }
        public int TumourGroupID
        {
            get
            {
                int tmp = 0; Int32.TryParse(TumourGroupStringID, out tmp); return tmp;
            }
            set { TumourGroupStringID = value.ToString(); }
        }

        public IList<String> Keywords { get; set; }

        //public IList<Reference> Reference { get; set; }

        public static QuestionViewModel CreateFromQuestion(Question q, int idx)
        {
            QuestionViewModel questionViewModel = new QuestionViewModel
            {
                QuestionID = q.ID,
                Index = idx,
                Delete = false,
                QuestionText = q.QuestionText,
                TimeTaken = q.TimeTaken,
                Response = q.Response,
                Probability = q.Probability,
                Severity = q.Severity,
                SpecialNotes = q.SpecialNotes,
                QuestionTypeID = q.QuestionType != null ? q.QuestionType.ID : 0,
                TumourGroupID = q.TumourGroup != null ? q.TumourGroup.ID : 0,
                Keywords = new List<string>(q.Keywords.Select(k => k.KeywordText)),
            };
            return questionViewModel;
        }

        public void MapToQuestion(Question q)
        {
            q.QuestionText = QuestionText;
            q.TimeTaken = TimeTaken;
            q.Response = Response;
            q.Probability = Probability;
            q.Severity = Severity;
            q.SpecialNotes = SpecialNotes;
        }
    }
}