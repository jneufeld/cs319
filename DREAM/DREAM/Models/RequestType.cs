using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DREAM.Models
{
    public class RequestType : DropDown
    {
        public static List<SelectListItem> getDropdownList()
        {
            List<SelectListItem> requestTypes = new List<SelectListItem>();

            requestTypes.Add(new SelectListItem { Text = "Administrator", Value = "ADMIN" });
            requestTypes.Add(new SelectListItem { Text = "Dietician", Value = "RDN" });
            requestTypes.Add(new SelectListItem { Text = "Drug Company", Value = "DRUG CO" });
            requestTypes.Add(new SelectListItem { Text = "Family Member", Value = "FAMILY" });
            requestTypes.Add(new SelectListItem { Text = "Family Physician", Value = "GP" });
            requestTypes.Add(new SelectListItem { Text = "General Public", Value = "PUB" });
            requestTypes.Add(new SelectListItem { Text = "Librarian", Value = "LIB" });
            requestTypes.Add(new SelectListItem { Text = "Naturopath", Value = "ND" });
            requestTypes.Add(new SelectListItem { Text = "News Media", Value = "MEDIA" });
            requestTypes.Add(new SelectListItem { Text = "Nurse", Value = "RN" });
            requestTypes.Add(new SelectListItem { Text = "Oncologist", Value = "ONC" });
            requestTypes.Add(new SelectListItem { Text = "Other Health Care Professional", Value = "HCP" });
            requestTypes.Add(new SelectListItem { Text = "Patient", Value = "PATIENT" });
            requestTypes.Add(new SelectListItem { Text = "Pharmacist", Value = "RX" });
            requestTypes.Add(new SelectListItem { Text = "Public Relations", Value = "PR" });
            requestTypes.Add(new SelectListItem { Text = "Radiation therapist", Value = "RT" });
            requestTypes.Add(new SelectListItem { Text = "Researcher", Value = "RESEARCH" });
            requestTypes.Add(new SelectListItem { Text = "Social worker", Value = "SW" });
            requestTypes.Add(new SelectListItem { Text = "Student", Value = "STUDENT" });
            requestTypes.Add(new SelectListItem { Text = "Unknown", Value = "OTHER" });

            return requestTypes;
        }
    }
}