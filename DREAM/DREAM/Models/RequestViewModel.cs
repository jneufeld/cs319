﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

        [Display(Name = "Request Type")]
        public string RequestTypeStringID { get; set; }

        public int CallerID { get; set; }

        [Display(Name = "First Name")]
        public string CallerFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string CallerLastName { get; set; }

        [Display(Name = "Phone Number")]
        public string CallerPhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string CallerEmail { get; set; }

        [Display(Name = "Region")]
        public string CallerRegionStringID { get; set; }

        public int PatientID { get; set; }

        [Display(Name = "Agency ID")]
        public int PatientAgencyID { get; set; }

        [Display(Name = "First Name")]
        public string PatientFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string PatientLastName { get; set; }

        [Display(Name = "Gender")]
        public string PatientGender { get; set; }

        [Display(Name = "Age")]
        public int PatientAge { get; set; }

        public IEnumerable<SelectListItem> RequestTypeDropDownList { get; set; }
        public IEnumerable<SelectListItem> RegionDropDownList { get; set; }
        public IEnumerable<SelectListItem> GenderDropDownList { get; set; }

        public static RequestViewModel CreateFromRequest(Request r)
        {
            RequestViewModel requestViewModel = new RequestViewModel
            {
                RequestID = r.ID,
                CreationTime = r.CreationTime.ToLocalTime().ToString(),
                CompletionTime = r.CompletionTime != null ? r.CompletionTime.Value.ToLocalTime().ToString() : "",
                //CreatedBy = Membership.GetUser(r.CreatedBy).UserName,
                //ClosedBy = Membership.GetUser(r.ClosedBy).UserName,
                RequestTypeStringID = r.Type != null ? r.Type.StringID : "",
                CallerID = r.Caller.ID,
                CallerFirstName = r.Caller.FirstName,
                CallerLastName = r.Caller.LastName,
                CallerPhoneNumber = r.Caller.PhoneNumber,
                CallerEmail = r.Caller.Email,
                CallerRegionStringID = r.Caller.Region != null ? r.Caller.Region.StringID : "",
                PatientID = r.Patient.ID,
                PatientAgencyID = r.Patient.AgencyID,
                PatientFirstName = r.Patient.FirstName,
                PatientLastName = r.Patient.LastName,
                PatientGender = r.Patient.Gender.ToString(),
                PatientAge = r.Patient.Age,
            };
            return requestViewModel;
        }

        public void MapToRequest(Request r)
        {
            r.ID = RequestID;
            int rtID;
            Int32.TryParse(RequestTypeStringID, out rtID);
            r.Type = new RequestType { ID = rtID };

            r.Caller.ID = CallerID;
            r.Caller.FirstName = CallerFirstName;
            r.Caller.LastName = CallerLastName;
            r.Caller.PhoneNumber = CallerPhoneNumber;
            r.Caller.Email = CallerEmail;
            int regID;
            Int32.TryParse(CallerRegionStringID, out regID);
            r.Caller.Region = new Region { ID = regID };
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
                r.Patient.Gender = gender;
            r.Patient.Age = PatientAge;
        }
    }
}