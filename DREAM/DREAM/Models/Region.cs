using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DREAM.Models
{
    public class Region : DropDown
    {
        public static List<SelectListItem> getDropdownList()
        {
            List<SelectListItem> requestTypes = new List<SelectListItem>();

            requestTypes.Add(new SelectListItem { Text = "Alberta", Value = "AB" });
            requestTypes.Add(new SelectListItem { Text = "Abbotsford Cancer Centre", Value = "AC" });
            requestTypes.Add(new SelectListItem { Text = "BC - Not Classified/Unknown", Value = "BCUNK" });
            requestTypes.Add(new SelectListItem { Text = "Centre for the Southern Interior", Value = "CSI" });
            requestTypes.Add(new SelectListItem { Text = "Fraser Health Region", Value = "FHR" });
            requestTypes.Add(new SelectListItem { Text = "Fraser Valley Region", Value = "FVC" });
            requestTypes.Add(new SelectListItem { Text = "Interior Health Region", Value = "IHR" });
            requestTypes.Add(new SelectListItem { Text = "Manitoba", Value = "MB" });
            requestTypes.Add(new SelectListItem { Text = "New Brunswick", Value = "NB" });
            requestTypes.Add(new SelectListItem { Text = "Newfoundland", Value = "NF" });
            requestTypes.Add(new SelectListItem { Text = "Northern Health Region", Value = "NOR" });
            requestTypes.Add(new SelectListItem { Text = "Nova Scotia", Value = "NS" });
            requestTypes.Add(new SelectListItem { Text = "Northwest Territories", Value = "NT" });
            requestTypes.Add(new SelectListItem { Text = "Nunavut", Value = "NU" });
            requestTypes.Add(new SelectListItem { Text = "Ontario", Value = "ON" });
            requestTypes.Add(new SelectListItem { Text = "Africa, Asia, Europe, USA", Value = "OTHER" });
            requestTypes.Add(new SelectListItem { Text = "Prince Edward Island", Value = "PE" });
            requestTypes.Add(new SelectListItem { Text = "Quebec", Value = "QC" });
            requestTypes.Add(new SelectListItem { Text = "Saskatchewan", Value = "SK" });
            requestTypes.Add(new SelectListItem { Text = "Not classified or unknown", Value = "UNK" });
            requestTypes.Add(new SelectListItem { Text = "Vancouver Cancer Centre", Value = "VCC" });
            requestTypes.Add(new SelectListItem { Text = "Vancouver Coastal Region", Value = "VCR" });
            requestTypes.Add(new SelectListItem { Text = "Vancouver Island Centre", Value = "VIC" });
            requestTypes.Add(new SelectListItem { Text = "Vancouver Island Region", Value = "VIHA" });
            requestTypes.Add(new SelectListItem { Text = "Yukon", Value = "YK" });

            return requestTypes;
        }
    }
}