using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DREAM.Models
{
    public enum Probability
    {
        Probable,
        Possible,
        Unlikely,
    }

    public enum Severity
    {
        Minor,
        Moderate,
        Major,
    }

    public class QuestionViewModel
    {
        public int QuestionID { get; set; }
        public string Index { get; set; }

        public bool Delete { get; set; }

        [Display(Name = "Question")]
        public string QuestionText { get; set; }

        [Display(Name = "Time Taken")]
        public int TimeTaken { get; set; }

        [Display(Name = "Response")]
        public string Response { get; set; }

        [Display(Name = "Probability")]
        public string Probability { get; set; }

        [Display(Name = "Severity")]
        public string Severity { get; set; }

        [Display(Name = "Impact")]
        public string Impact { get; set; }

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

        public IList<KeywordViewModel> Keywords { get; set; }

        public IList<ReferenceViewModel> References { get; set; }

        public static QuestionViewModel CreateFromQuestion(Question q, int idx)
        {
            QuestionViewModel questionViewModel = new QuestionViewModel
            {
                QuestionID = q.ID,
                Index = idx.ToString(),
                Delete = false,
                QuestionText = q.QuestionText,
                TimeTaken = q.TimeTaken,
                Response = q.Response,
                Probability = ((Probability)q.Probability).ToString(),
                Severity = ((Severity)q.Severity).ToString(),
                SpecialNotes = q.SpecialNotes,
                QuestionTypeID = q.QuestionType != null ? q.QuestionType.ID : 0,
                TumourGroupID = q.TumourGroup != null ? q.TumourGroup.ID : 0,
                Keywords = new List<KeywordViewModel>(q.Keywords.Select(k => KeywordViewModel.CreateFromKeyword(k))),
                References = new List<ReferenceViewModel>(q.References.Select(r => ReferenceViewModel.CreateFromReference(r))),
            };
            return questionViewModel;
        }

        public void MapToQuestion(Question q)
        {
            q.QuestionText = QuestionText;
            q.TimeTaken = TimeTaken;
            q.Response = Response;

            Probability p;
            Enum.TryParse(Probability, true, out p);
            q.Probability = (int)p;

            Severity s;
            Enum.TryParse(Severity, true, out s);
            q.Severity = (int)s;
            
            q.SpecialNotes = SpecialNotes;
        }
    }

    public class KeywordViewModel
    {
        public string Keyword { get; set; }

        public bool Delete { get; set; }

        public static KeywordViewModel CreateFromKeyword(Keyword k)
        {
            KeywordViewModel keywordViewModel = new KeywordViewModel
            {
                Keyword = k.KeywordText,
            };
            return keywordViewModel;
        }
    }

    public class ReferenceViewModel
    {
        public int ReferenceID { get; set; }

        public bool Delete { get; set; }

        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Reference Type")]
        public string ReferenceType { get; set; }

        public static ReferenceViewModel CreateFromReference(Reference r)
        {
            ReferenceViewModel referenceViewModel = new ReferenceViewModel
            {
                ReferenceID = r.ID,
                Text = r.Value,
                ReferenceType = ((ReferenceType)r.ReferenceType).ToString(),
            };
            return referenceViewModel;
        }

        public void MapToReference(Reference r)
        {
            r.ID = ReferenceID;
            r.Value = Text;
            ReferenceType type;
            bool parse = Enum.TryParse(ReferenceType, true, out type);
            if (parse)
                r.ReferenceType = (int)type;
        }
    }
}