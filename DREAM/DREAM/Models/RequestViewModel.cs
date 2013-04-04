using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataAnnotationsExtensions;

namespace DREAM.Models
{
    public class RequestViewModel
    {
        [Display(Name = "Request ID")]
        public int RequestID { get; set; }

        [Display(Name = "Date Created")]
        public string CreationTime { get; set; }

        [Display(Name = "Date Completed")]
        public string CompletionTime { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Closed By")]
        public string ClosedBy { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Requester Type")]
        public string RequesterTypeStringID { get; set; }
        public int RequesterTypeID
        {
            get
            { 
                int tmp = 0; Int32.TryParse(RequesterTypeStringID, out tmp); return tmp;
            }
            set { RequesterTypeStringID = value.ToString(); }
        }

        [Display(Name = "Requester Type")]
        public string RequesterTypeString { get; set; }

        public int CallerID { get; set; }

        [Display(Name = "First Name")]
        public string CallerFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string CallerLastName { get; set; }

        [Display(Name = "Phone Number")]
        //source: http://stackoverflow.com/questions/123559/a-comprehensive-regex-for-phone-number-validation
        [RegularExpression(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessage = "Entered phone format is not valid.")]
        public string CallerPhoneNumber { get; set; }

        [Email]
        [Display(Name = "Email")]
        public string CallerEmail { get; set; }

        [Display(Name = "Region")]
        public string CallerRegionStringID { get; set; }
        public int CallerRegionID
        {
            get
            {
                int tmp = 0; Int32.TryParse(CallerRegionStringID, out tmp); return tmp;
            }
            set { CallerRegionStringID = value.ToString(); }
        }

        [Display(Name = "Region")]
        public string CallerRegionString { get; set; }

        public int PatientID { get; set; }

        [Digits]
        [Display(Name = "Agency ID")]
        public string PatientAgencyID { get; set; }

        [Display(Name = "First Name")]
        public string PatientFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string PatientLastName { get; set; }

        [Display(Name = "Gender")]
        public string PatientGender { get; set; }

        [Range(0, 200)]
        [Display(Name = "Age")]
        public int PatientAge { get; set; }

        public string Action { get; set; }

        public int QuestionCount { get; set; }
        public IList<QuestionViewModel> Questions { get; set; }

        #region Constructor Methods
        public static RequestViewModel CreateFromRequest(Request r)
        {
            MembershipUser createdBy = Membership.GetUser(r.CreatedBy);
            MembershipUser closedBy = Membership.GetUser(r.ClosedBy);
            RequestViewModel requestViewModel = new RequestViewModel
            {
                RequestID = r.ID,
                CreationTime = r.CreationTime.ToLocalTime().ToString(),
                CompletionTime = r.CompletionTime != null ? r.CompletionTime.Value.ToLocalTime().ToString() : "",
                RequesterTypeID = r.Caller.Type != null ? r.Caller.Type.ID : 0,
                RequesterTypeString = r.Caller.Type != null ? r.Caller.Type.ToString() : "",
                Status = r.CompletionTime != null ? "Closed" : "Open",
                CallerID = r.Caller.ID,
                CallerFirstName = r.Caller.FirstName,
                CallerLastName = r.Caller.LastName,
                CallerPhoneNumber = r.Caller.PhoneNumber,
                CallerEmail = r.Caller.Email,
                CallerRegionID = r.Caller.Region != null ? r.Caller.Region.ID : 0,
                CallerRegionString = r.Caller.Region != null ? r.Caller.Region.FullName : "",
                PatientID = r.Patient.ID,
                PatientAgencyID = r.Patient.AgencyID,
                PatientFirstName = r.Patient.FirstName,
                PatientLastName = r.Patient.LastName,
                PatientGender = ((Gender)r.Patient.Gender).ToString(),
                PatientAge = r.Patient.Age,
                Questions = new List<QuestionViewModel>(),
                CreatedBy = createdBy != null ? createdBy.UserName : "",
                ClosedBy = closedBy != null ? closedBy.UserName : "",
            };
            int idx = 0;
            foreach (Question q in r.Questions)
            {
                requestViewModel.Questions.Add(QuestionViewModel.CreateFromQuestion(q, idx));
                idx++;
            }
            requestViewModel.QuestionCount = idx;
            return requestViewModel;
        }

        public void MapToRequest(Request r)
        {
            r.ID = RequestID;

            r.Caller.ID = CallerID;
            r.Caller.FirstName = CallerFirstName;
            r.Caller.LastName = CallerLastName;
            r.Caller.PhoneNumber = CallerPhoneNumber;
            r.Caller.Email = CallerEmail;
        }

        public void MapToRequestPatient(Request r)
        {
            r.Patient.ID = PatientID;
            r.Patient.AgencyID = PatientAgencyID;
            r.Patient.FirstName = PatientFirstName;
            r.Patient.LastName = PatientLastName;
            Gender gender;
            bool parse = Enum.TryParse(PatientGender, true, out gender);
            if (parse)
                r.Patient.Gender = (int)gender;
            r.Patient.Age = PatientAge;
        }
        #endregion
    }
}